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
    render: { type: Boolean, default: false },
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
      centerView: undefined,
      userPosition: this.$cookiz.get('userPosition'),
      contentLocationId: null,
      showLayers: [],
      aLayer: '',
      latitude: '',
      longitude: '',
      title: undefined,
      mapHelper: null,
      positions: [
        {
        }
      ]
    }
  },
  watch: {
    render (oldValue, newValue) {
      if (this.render === true) {
        this.reCenter()
      }
    }
  },
  mounted () {
    this.activate()
  },
  methods: {
    reCenter () {
      this.centerView = this.userPosition.coordinates
      console.log(this.centerView)
    },
    activate () {
      setTimeout(() => {
        this.renderChart(this.locations)
      }, 1000)
    },

    renderChart (locations) {
      // const centerView = this.$ol.format.fromLonLat(locations && locations.length > 0
      //   ? [(locations[0].longitude),
      //       (locations[0].latitude)]
      //   : [12, 54])

      if (this.userPosition.coordinates && this.userPosition.coordinates.length > 0) {
        this.centerView = this.$ol.format.fromLonLat([this.userPosition.coordinates[1],
          this.userPosition.coordinates[0]])
      } else if (this.$ol.format.fromLonLat(locations && locations.length > 0)) {
        this.centerView = this.$ol.format.fromLonLat([(locations[0].longitude),
          (locations[0].latitude)])
      } else {
        this.centerView = this.$ol.format.fromLonLat([12, 54])
      }
      const centerView = this.centerView

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
      this.mapHelper.map.on('click', (e) => {

      })

      // let stopPropagation = false
      this.mapHelper.map.on('click', (e) => {
        // if (stopPropagation) {
        //   // return false
        // }

        this.mapHelper.map.forEachFeatureAtPixel(e.pixel, (feature) => {
          this.$router.push({ path: `./${feature.values_.setWalkId}` })
        })
      })

      let walkId = -1

      // reads the current locations and sends to method to add them to the map
      for (const location of locations) {
        const lonLat = this.$ol.format.fromLonLat([(location.longitude), (location.latitude)])
        this.title = location.title
        walkId = location.walkId

        this.addCurrentPosition(`${location.id}`, lonLat, this.typeOfLayer, walkId)
      }
    },
    // adds all the current locations to the map
    addCurrentPosition (layerId, [lon, lat], typeOfLayer, walkId) { // typeOfLayer = position, label, user
      const createdLayer = this.mapHelper.addLayer(layerId, typeOfLayer, walkId, lon, lat)
      this.mapHelper.addPosition(createdLayer, this.title)
      this.mapHelper.createLabel(createdLayer, lon, lat)
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
