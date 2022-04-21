<template>
  <div>
    <v-dialog
      v-model="contentDialog"
      persistent
      max-width="600px"
    >
      <ContentDialog
        :location-content="locationContent"
        @dialog-close="[contentDialog = false, locationDone = true, showNext = true ]"
      />
    </v-dialog>
    <v-btn
      v-if="notPlaying"
      class="success"
      @click="runGame()"
    >
      Start the Game
    </v-btn>
    <v-btn
      v-if="gameInProgress"
      class="red"
      @click="quitGame()"
    >
      Quit
    </v-btn>
    <v-btn
      v-if="showNext"
      class="success"
      @click="runGame()"
    >
      Show next location
    </v-btn>
    <v-btn v-if="notPlaying" class="blue back-button" @click="goBack()">
      Back
    </v-btn>
    <div class="map-container">
      <ClientOnly>
        <MapContainerUser
          ref="mapContainerGame"
          :locations="locations"
          :set-zoom="zoom"
        />
      </ClientOnly>
    </div>
  </div>
</template>

<script>
import MapContainerUser from '../manualComponents/MapContainerUser.vue'

import 'ol/ol.css'

export default {
  components: {
    MapContainerUser
  },
  props: {
    selectedWalkId: { type: Number, default: null }
  },

  data () {
    return {
      currentLocation: undefined,
      locations: this.locations,
      contentDialog: false,
      locationContent: undefined,
      zoom: 17,
      closeToPoint: false,
      correctAnswer: false,
      doneClicked: false,
      notPlaying: true,
      gameInProgress: false,
      quitPressed: false,
      noMoreContent: false,
      locationDone: false,
      showNext: false,
      visitedLocations: []
      // this.$refs.mapContainer.mapHelper.map.values_.layergroup.values_.layers.array_
      // mapPoints: this.$store.map.map.values_.layergroup.values_.layers.array_
    }
  },
  created () {
    this.GetWalk()
    // console.log(this.mapPoints)
    // console.log(this.$store.map)
  },
  methods: {
    GetWalk () {
      this.$axios
        .get(`/walk/${this.selectedWalkId}`)
        .then((res) => {
          this.walk = res.data
          this.locations = res.data.locations.map(x => ({ ...x, visible: false }))
          // this.$emit('walkId')
        })
    },
    goBack () {
      this.$router.go(-1)
    },
    runGame () {
      this.gameInProgress = true
      this.notPlaying = false

      this.showNextLocation()
    },

    showNextLocation () {
      // show user dot and start tracking

      if (this.locations.length > 0) {
        // adds the first location in the array to the visited ones
        this.locationContent = this.locations[0]
        this.locations[0].visible = true
        this.$refs.mapContainerGame.renderChart()

        setTimeout(() => {
          this.closeToPoint = true
          if (this.closeToPoint) {
            this.locationDone = false// - if user is close enough to location open dialog

            this.locationDone = this.showContentDialog(this.locations[0])
            this.visitedLocations.push(this.locations[0])
            this.locations.shift(0) // removes the location
          }
        }, 2000)
        // logic to show content and if the location is completed, conditional etc to set the below
      } else if (this.locationDone) {
        this.showNext = false
        this.gameInProgress = false
        console.log('no more locations')
        this.quitGame()
      }
    },
    showContentDialog () {
      // show content, wait for the right answer
      this.$nuxt.$emit('getContent')
      this.contentDialog = true
      console.log('done')
      if (this.locationDone) {
        return true
      }
    },

    quitGame () {
      this.quitPressed = true
      this.gameInProgress = false
      this.notPlaying = true
      this.visitedLocations = []
      this.GetWalk()
      console.log('quit')
    }
  }
}
</script>

<style>

</style>
