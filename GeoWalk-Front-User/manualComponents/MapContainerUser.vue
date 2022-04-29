<template>
  <div>
    <v-container>
      <v-row>
        <v-col
          cols="12"
          sm="6"
          md="4"
        />
        <v-col
          cols="12"
          sm="6"
          md="4"
        />
      </v-row>
    </v-container>

    <div id="map" ref="map" />
  </div>
</template>
<script>

export default {
  name: 'Map',
  props: {
    user: { type: Array, default: undefined },
    locationRemoved: { type: Number, default: null },
    locations: { type: Array, default: () => [] },
    setZoom: { type: Number, default: null },
    walks: { type: Array, default: () => [] }
  },
  data () {
    return {

      pointerCoordinates: [
        {

        }
      ],
      optionalZoom: undefined,
      centerView: [],
      userPosition: undefined,
      contentLocationId: null,
      showLayers: [],
      aLayer: '',
      latitude: '',
      longitude: '',
      title: undefined,
      mapHelper: null,
      isVisible: true,
      positions: [
        {
        }
      ]
    }
  },
  // watch: {

  //   userPosition (old, value) {
  //     console.log(old, value)
  //   }
  // },
  mounted () {
    this.activate()
    this.$nuxt.$on('updateCenterView', () => {
      this.reCenter()
    })
  },
  methods: {
    reCenter () {
      // if (this.$refs.map) { this.$refs.map.innerHTML = '' }
      this.userPosition = this.$cookiz.get('userPosition')
      console.log(this.$cookiz.get('userPosition'))
      // this.$cookiz.remove('userPosition')
      this.$cookiz.set('userPosition', undefined, { maxAge: 0 })
      console.log(this.$cookiz.get('userPosition'))
      this.renderChart()
    },
    activate () {
      if (this.locations.length > 0) {
        this.renderChart() // necessary to allow the fetch to finish on locations before reading lat and lon
      } else {
        setTimeout(() => {
          this.renderChart() // necessary to allow the fetch to finish on locations before reading lat and lon
        }, 1500)
      }
    },
    renderChart () {
      this.$refs.map.innerHTML = ''
      // this.locations.map(x => ({ ...x, visible: true }))
      if (this.userPosition) {
        this.centerView = this.$ol.format.fromLonLat([this.userPosition.lon,
          this.userPosition.lat])

        this.$store.user = this.$ol.format.fromLonLat([this.userPosition.lon,
          this.userPosition.lat])
        this.$store.userLonLat = [this.userPosition.lon, this.userPosition.lat]
      } else if (this.locations && this.locations.length > 0) {
        this.$ol.format.fromLonLat(this.locations && this.locations.length)
        this.centerView = this.$ol.format.fromLonLat([(this.locations[0].longitude),
          (this.locations[0].latitude)])
      } else {
        this.centerView = this.$ol.format.fromLonLat([14, 56])
        this.optionalZoom = 7
      }
      const centerView = this.centerView

      const options = {
        target: this.$refs.map,
        zoom: this.optionalZoom ?? this.setZoom,
        centerView,
        layers: [
          // adding a background tiled layer
          new this.$ol.layer.TileLayer({

            source: new this.$ol.source.OSM() // tiles are served by OpenStreetMap
          })

          // vectorLayer,
        ]

      }
      this.mapHelper = this.$ol.createMap(options)
      // this.mapHelper.map.on('click', (e) => {

      // })

      // let stopPropagation = false
      this.mapHelper.map.on('click', (e) => {
        this.mapHelper.map.forEachFeatureAtPixel(e.pixel, (feature) => {
          this.$router.push({ path: `./${feature.values_.setWalkId}` })
        })
      })

      let walkId = -1

      // reads the current locations and sends to method to add them to the map
      for (const location of this.locations) {
        const lonLat = this.$ol.format.fromLonLat([(location.longitude), (location.latitude)])
        this.title = location.title
        walkId = location.walkId
        this.isVisible = location.visible

        this.addCurrentPosition(`${location.id}`, lonLat, this.typeOfLayer, walkId, this.isVisible)
      }
      this.addUserPosition(null, this.$store.user, 'userlayer', null, true)
      this.$store.map = this.mapHelper
    },
    // adds all the current locations to the map
    addCurrentPosition (layerId, [lon, lat], typeOfLayer, walkId, isVisible) { // typeOfLayer = position, label, user
      const createdLayer = this.mapHelper.addLayer(layerId, typeOfLayer, walkId, lon, lat, isVisible)
      this.mapHelper.addPosition(createdLayer, this.title)
      this.mapHelper.createLabel(createdLayer, lon, lat)
    },
    addUserPosition (layerId, [lon, lat], typeOfLayer, walkId, isVisible) {
      const createdLayer = this.mapHelper.addLayer(layerId, typeOfLayer, walkId, lon, lat, isVisible)
      this.mapHelper.addPosition(createdLayer, 'User')
      this.mapHelper.createLabel(createdLayer, lon, lat)
      console.log(createdLayer)
    }

  }
}

</script>
<style >
#map {
 max-width:1200px;
 min-width:500px;
 height:840px;
}
.dialog-required-message{
  font-size: 12px;
  color:red
}
</style>
