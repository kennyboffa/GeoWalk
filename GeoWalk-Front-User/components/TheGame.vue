<template>
  <div>
    <v-dialog
      v-model="contentDialog"
      persistent
      max-width="600px"
    >
      <ContentDialog
        :location-content="locationContent"
        @dialog-close="contentDialog = false"
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
  props: [
    'selectedWalkId'
  ],
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

      // if (this.showNext) {
      //   this.showLocationContent()
      // }
      // if (this.showNext && this.gameInProgress) {
      //   this.runGame()
      // } else if (!this.showNext) {
      //   this.gameInProgress = false
      // }
      // } else if (this.noMoreContent === true) {
      //   this.gameInProgress = false
      // }
    },

    showNextLocation () {
      // show user dot and start tracking

      if (this.locations.length > 0) {
        // adds the first location in the array to the visited ones

        // let timeout = 1000 // simulates the game
        for (const location of this.locations) {
          // setTimeout(() => {
          location.visible = true
          this.$refs.mapContainerGame.renderChart()

          this.closeToPoint = true
          if (this.closeToPoint) {
            this.locationDone = false// - if user is close enough to location open dialog
            this.showContentDialog(location)
          }

          // logic to show content and if the location is completed, conditional etc to set the below
          this.locationDone = true

          if (this.locationDone) {
            console.log('done')
            this.visitedLocations.push(this.locations[0])
            this.locations.shift(0) // removes the location
          }
          // }, timeout)
          // timeout += 1000
        }

        this.showNext = true
      } else if (this.locationDone) {
        this.showNext = false
        this.gameInProgress = false
        console.log('no more locations')
        this.quitGame()
      }
    },
    showContentDialog (location) {
      // show content, wait for the right answer
      this.locationContent = location
      this.contentDialog = true
      console.log(location)

      if (this.correctAnswer || this.doneClicked) {
        console.log('asd')
        this.locationDone = true // set location done in showContentDialog
      } else { this.locationDone = false }
    },

    quitGame () {
      this.quitPressed = true
      this.gameInProgress = false
      this.notPlaying = true
      this.visitedLocations = []
      this.GetWalk()
      console.log('quit')
    },
    showLocationContent () {
      // show dialog with fact/questions
      // add logic to check if user visited location and finished task
      console.log('content')
      this.showNext = true
      // if location is visited and "done" return true
    }
  }
}
</script>

<style>

</style>
