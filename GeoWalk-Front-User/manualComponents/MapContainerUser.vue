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
      this.$cookiz.remove('userPosition')
      this.renderChart()
    },
    activate () {
      setTimeout(() => {
        this.renderChart()
      }, 300)
    },

    renderChart () {
      this.$refs.map.innerHTML = ''
      if (this.userPosition) {
        this.centerView = this.$ol.format.fromLonLat([this.userPosition.lon,
          this.userPosition.lat])
        console.log('user')
      } else if (this.$ol.format.fromLonLat(this.locations && this.locations.length > 0)) {
        this.centerView = this.$ol.format.fromLonLat([(this.locations[0].longitude),
          (this.locations[0].latitude)])
        console.log('loc')
      } else {
        this.centerView = this.$ol.format.fromLonLat([12, 54])
      }
      const centerView = this.centerView
      console.log(centerView)

      const options = {
        target: this.$refs.map,
        zoom: this.setZoom,
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
    },
    // adds all the current locations to the map
    addCurrentPosition (layerId, [lon, lat], typeOfLayer, walkId, isVisible) { // typeOfLayer = position, label, user
      const createdLayer = this.mapHelper.addLayer(layerId, typeOfLayer, walkId, lon, lat, isVisible)
      this.mapHelper.addPosition(createdLayer, this.title)
      this.mapHelper.createLabel(createdLayer, lon, lat)
      this.$store.map = this.mapHelper
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
