<template>
  <div>
    {{ aLayer }}
    <v-tooltip>
      <template #activator="{ on }">
        <v-btn v-if="showToolTip" color="primary" dark top v-on="on">
          Top
        </v-btn>
      </template>
      <span>Top tooltip</span>
    </v-tooltip>

    <v-dialog
      v-model="dialog"
      persistent
      max-width="600px"
    >
      <v-card>
        <v-card-title>
          <span class="text-h5">Add a Location</span>
        </v-card-title>
        <v-card-text>
          <v-container>
            <v-row>
              <v-col
                cols="12"
                sm="6"
                md="4"
              >
                <v-text-field
                  v-model="locationTitle"
                  label="Location Title"
                  value=":this.locationTitle"
                  required
                />
                <v-text-field
                  v-model="latitude"
                  label="Location Latitude"
                  value=":this.latitude"
                  required
                />
                <v-text-field
                  v-model="longitude"
                  label="Location Longitude"
                  value=":this.longitude"
                  required
                />
              </v-col>
              <v-col
                cols="12"
                sm="6"
                md="4"
              />
            </v-row>
          </v-container>
          <small>*indicates required field</small>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn
            color="blue darken-1"
            text
            @click="dialog=false"
          >
            Cancel
          </v-btn>
          <v-btn
            color="blue darken-1"
            text
            @click="PostLocation()"
          >
            Save
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <div id="map" ref="map" />
  </div>
</template>
<script>

export default {
  name: 'Map',
  props: {
    locationRemoved: { type: Number, default: null },
    selectedWalkId: { type: Number, default: null },
    locations: { type: Array, default: () => [] }
  },
  data () {
    return {

      pointerCoordinates: [
        {

        }
      ],
      showToolTip: true,
      showLayers: [],
      aLayer: '',
      dialog: false,
      latitude: '',
      longitude: '',
      locationTitle: '',
      mapHelper: null,
      positions: [
        {
          // latitude: this.latitude,
          // longitude: this.longitude,
          // title: '',
          // description: '',
          // contents: []
        }
      ]
    }
  },
  watch: {
    locationRemoved (locationId) {
      if (locationId) {
        this.mapHelper.removeLayerFromMap(`location_${locationId}`)
        // this.renderChart(this.locations) // den körs, men visar även de som tagits bort.
      }
    }
  },
  mounted () {
    this.renderChart(this.locations)
  },
  methods: {

    renderChart (locations) {
      const centerView = this.$ol.format.fromLonLat(locations && locations.length > 0 ? [(locations[0].longitude), (locations[0].latitude)] : [12, 54])
      const options = {
        target: this.$refs.map,
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

      // reads the current locations and sends to method to add them to the map
      for (const location of locations) {
        const lonLat = this.$ol.format.fromLonLat([(location.longitude), (location.latitude)])
        this.addCurrentPosition(`location_${location.id}`, lonLat)
      }
      this.mapHelper.map.on('click', (e) => {
        const clickedCoordinates = this.$ol.format.toLonLat(e.coordinate)
        this.addNewPosition(clickedCoordinates)
      })

      let stopPropagation = false
      this.mapHelper.map.on('click', (e) => {
        if (stopPropagation) {
          return false
        }
        console.log(e)
        console.log(Object.keys(e))

        this.mapHelper.map.forEachFeatureAtPixel(e.pixel, (feature) => {
          if (feature.values_.layerName.length > 0) {
            stopPropagation = true
            let locationId = feature.values_.layerName.split('.')
            locationId = feature.values_.layerName.split('_')
            this.$router.push({ path: `/location/${locationId[1]}` })
          }
        })
      })
    },
    // adds all the current locations to the map
    addCurrentPosition (layerName, [lon, lat]) {
      this.mapHelper.addPosition(layerName, lon, lat)
      this.mapHelper.createLabel(layerName, lon, lat)
      // this.currentPositions.push([lon, lat])
    },

    // adds clicked positions after saving in dialog
    addNewPosition ([lon, lat]) {
      this.latitude = lat
      this.longitude = lon
      this.dialog = true
      this.title = this.locationTitle
    },
    PostLocation () {
      const newLocation = {
        walkId: this.selectedWalkId,
        title: this.locationTitle,
        longitude: this.longitude,
        latitude: this.latitude
      }
      this.$axios
        .post('/location', newLocation)
        .then((res) => {
          this.dialog = false
          const lonLat = this.$ol.format.fromLonLat([(res.data.longitude), (res.data.latitude)])
          this.addCurrentPosition(`location_${res.data.id}`, lonLat)
          this.$emit('location-added')
        })
    }

  }
}

</script>
<style >
#map {
 width:1000px;
 height:800px;
}
</style>
