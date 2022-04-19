<template>
  <div>
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
      locations: this.locations,
      zoom: 16,
      closeToPoint: false,
      correctAnswer: false,
      doneClicked: false,
      notPlaying: true,
      gameInProgress: false,
      quitPressed: false,
      noMoreContent: false,
      locationDone: false,
      showNext: false,
      visitedLocations: [],
      visitedMapPoints: []
      // mapPoints: this.$refs.mapContainer.mapHelper.map.values_.layergroup.values_.layers.array_
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
          this.locations = res.data.locations
          // this.$emit('walkId')
        })
    },
    goBack () {
      this.$router.go(-1)
    },
    runGame () {
      this.gameInProgress = true
      this.notPlaying = false
      console.log(this.$refs.mapContainerGame)
      this.showNextLocation()

      // setTimeout(() => {
      //   this.showNextLocation()
      // }, 1000)

      if (this.showNext) {
        this.showLocationContent()
      }
      if (this.showNext && this.gameInProgress) {
        this.runGame()
      } else if (!this.showNext) {
        this.gameInProgress = false
      }
      // } else if (this.noMoreContent === true) {
      //   this.gameInProgress = false
      // }
    },

    showNextLocation () {
      this.locationDone = false

      if (this.closeToPoint) { // - if user is close enough to location open dialog
        this.showContentDialog(this.locationId)
      }

      if (this.locations.length > 0) {
        // this.visitedLocations.push(this.locations[0]) // adds the first location in the array to the visited ones

        // this.visitedMapPoints.push(this.mapPoints[1]) // adds the point
        // this.visitedMapPoints.push(this.mapPoints[2]) // adds the label
        // this.visitedMapPoints[0].values_.visible = true
        // console.log(this.visitedMapPoints)
        if (this.$refs.mapContainerGame) {
          this.$refs.mapContainerGame.$refs.map.innerHTML = ''

          this.$refs.mapContainerGame._props.locations[1] = ''
          // this.$refs.mapContainerGame.mapHelper.map.values_.layergroup.values_.layers.array_[0].values_.visible = false
          // this.$refs.mapContainerGame.mapHelper.map.values_.layergroup.values_.layers.array_[1].values_.visible = false
          // this.$refs.mapContainerGame.mapHelper.map.values_.layergroup.values_.layers.array_[2].values_.visible = false
          // this.$refs.mapContainerGame.mapHelper.map.values_.layergroup.values_.layers.array_[3].title = ''
          this.$refs.mapContainerGame.renderChart()
          console.log(this.$refs.mapContainerGame.mapHelper.map.values_.layergroup.values_.layers.array_[3])
          // console.log(this.mapPoints[1].values_)
        }

        this.locationDone = true // just to stop the loop

        // if (this.locationDone) {
        //   this.locations.shift(0) // removes the location
        //   this.mapPoints.shift(1) // removes the point
        //   this.mapPoints.shift(2) // removes the label
        // }

        console.log('update the map')
        this.showNext = false
      } else {
        this.showNext = false
        this.gameInProgress = false
        console.log('no more locations')
        this.quitGame()
      }
    },
    showContentDialog (locationId) {
      // show content, wait for the right answer
      if (this.correctAnswer || this.doneClicked) {
        this.locationDone = true // set location done in showContentDialog
      }
    },

    quitGame () {
      this.quitPressed = true
      this.gameInProgress = false
      this.notPlaying = true
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
