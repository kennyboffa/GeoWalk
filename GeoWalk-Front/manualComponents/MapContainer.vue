<template>
  <div>
    <v-dialog
      v-model="addContentDialog"
      persistent
      max-width="600px"
    >
      <ContentDialog
        :content-location-id="contentLocationId"
        @dialog-close="addContentDialog = false"
      />
    </v-dialog>
    <v-dialog
      v-model="addLocationDialog"
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
                  v-model="title"
                  label="* Location Title"
                  value=":this.title"
                  required
                />
                <div v-if="title === undefined || title === '' " class="dialog-required-message">
                  Title has to be set
                </div>
                <!-- <v-text-field
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
                /> -->
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
            @click="addLocationDialog=false"
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
      contentLocationId: null,
      typeOfLayer: 'undefined', // skapa typ en checkbox för vilken typ det ska vara i dialog. - position, label, fun facts? showUser etc
      showToolTip: true,
      showLayers: [],
      aLayer: '',
      addContentDialog: false,
      addLocationDialog: false,
      latitude: '',
      longitude: '',
      title: undefined,
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
        this.mapHelper.removeLayerFromMap(`${locationId}`)
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
        this.title = location.title
        this.addCurrentPosition(`${location.id}`, lonLat)
      }
      this.mapHelper.map.on('click', (e) => {
        const clickedCoordinates = this.$ol.format.toLonLat(e.coordinate)
        this.addNewPosition(clickedCoordinates)
      })

      // let stopPropagation = false
      this.mapHelper.map.on('click', (e) => {
        // if (stopPropagation) {
        //   // return false
        // }

        this.mapHelper.map.forEachFeatureAtPixel(e.pixel, (feature) => {
          if (feature.values_.layerId) {
            this.addLocationDialog = false
            // stopPropagation = true
            this.addContentDialog = true
            this.contentLocationId = parseInt(feature.values_.layerId)
          }
        })
      })
    },
    // adds all the current locations to the map
    addCurrentPosition (layerId, [lon, lat], typeOfLayer) { // typeOfLayer = position, label, user
      const createdLayer = this.mapHelper.addLayer(layerId, typeOfLayer, lon, lat)
      this.mapHelper.addPosition(createdLayer, this.title)
      this.mapHelper.createLabel(createdLayer, lon, lat)
    },

    // adds clicked positions after saving in dialog
    addNewPosition ([lon, lat]) {
      this.latitude = lat
      this.longitude = lon
      this.addLocationDialog = true
    },
    PostLocation () {
      const newLocation = {
        walkId: this.selectedWalkId,
        title: this.title,
        longitude: this.longitude,
        latitude: this.latitude,
        typeOfLayer: this.typeOfLayer

      }
      if (this.title !== undefined && this.title !== '') {
        this.$axios
          .post('/location', newLocation)
          .then((res) => {
            this.addLocationDialog = false
            const lonLat = this.$ol.format.fromLonLat([(res.data.longitude), (res.data.latitude)])
            this.addCurrentPosition(`${res.data.id}`, lonLat)
            this.$emit('location-added')
            this.title = undefined
          })
      }
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
