<template>
  <div>
    <v-dialog
      v-model="contentDialog"
      persistent
      max-width="600px"
    >
      <ContentDialog
        v-if="locationContent"
        :quit-pressed="quitPressed"
        :visited-locations="visitedLocations"
        :location-content="locationContent"
        @dialog-close="[contentDialog = false, locationDone = true, showNext = true ]"
      />
    </v-dialog>
    <v-btn
      v-if="!gameInProgress"
      class="success"
      @click="[visitedLocations = [], runGame()]"
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
      v-if="showNext && gameInProgress"
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
          :user="user"
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
    selectedWalkId: { type: String, default: null }
  },

  data () {
    return {
      intervalId: undefined,
      metresToPoint: undefined,
      localTime: undefined,
      user: this.$store.userLonLat,
      walkId: parseInt(this.selectedWalkId),
      currentLocation: undefined,
      locations: [],
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
    }
  },

  created () {
    this.GetWalk()
  },
  methods: {
    updateUserPosition () {
      this.intervalId = setInterval(() => {
        this.$nuxt.$emit('gameRunning')
        console.log('running')
        this.metresToPoint = this.userToLocationInMetres()
        console.log(this.metresToPoint)
        if (this.metresToPoint < 40) {
          this.closeToPoint = true
        } else { this.closeToPoint = false }
      }, 2000)
    },

    GetWalk () {
      this.$axios
        .get(`/walk/${this.walkId}`)
        .then((res) => {
          this.walk = res.data
          this.locations = res.data.locations.map(x => ({ ...x, visible: false }))
          // this.$refs.mapContainerGame.renderChart(true)
        })
    },
    goBack () {
      this.$router.go(-1)
    },
    runGame () {
      this.gameInProgress = true
      if (this.gameInProgress && !this.closeToPoint) {
        this.updateUserPosition()
      }
      this.showNextLocation()
    },

    userToLocationInMetres () { // generally used geo measurement function
      const R = 6378.137 // Radius of earth in KM
      const dLat = this.user[1] * Math.PI / 180 - this.locationContent.latitude * Math.PI / 180
      const dLon = this.user[0] * Math.PI / 180 - this.locationContent.longitude * Math.PI / 180
      const a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
    Math.cos(this.user[1] * Math.PI / 180) * Math.cos(this.locationContent.latitude * Math.PI / 180) *
    Math.sin(dLon / 2) * Math.sin(dLon / 2)
      const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a))
      const d = R * c
      return d * 1000 // meters
    },
    showNextLocation () {
      // add logic to show user dot and start tracking

      if (this.locations.length > 0) {
        // adds the first location in the array to the visited ones
        this.quitPressed = false
        this.locationContent = this.locations[0]
        console.log(this.locationContent)
        this.visitedLocations.push(this.locations[0])
        this.locations[0].visible = true
        this.$refs.mapContainerGame.renderChart()
        this.locations.shift(0) // removes the Location
        console.log(this.user)

        setTimeout(() => {
          this.closeToPoint = true // ACHTUNG!! REMOVE when userpositions updates properly, only for testing!!
          if (this.closeToPoint) {
            this.locationDone = false// - if user is close enough to location open dialog
            clearInterval(this.intervalId)
            this.showContentDialog()
          }
        }, 500)

        // logic to show content and if the location is completed, conditional etc to set the below
      } else if (this.locationDone && this.locations.length === 0) {
        this.showNext = false
        this.gameInProgress = false
        this.locationContent = {}
        console.log('no more locations')
        clearInterval(this.intervalId)
        this.quitGame()
      }
    }, // logic below more adjusted to the real game
    // runGame () {
    //   this.gameInProgress = true
    //   if (this.gameInProgress && !this.closeToPoint) {
    //     this.updateUserPosition()
    //   } else if (this.closeToPoint) {
    //     setTimeout(() => {
    //       this.locationDone = false// - if user is close enough to location open dialog

    //       this.showContentDialog()
    //     }, 500)

    //     // logic to show content and if the location is completed, conditional etc to set the below
    //   } else if (this.locationDone && this.locations.length === 0) {
    //     this.showNext = false
    //     this.gameInProgress = false
    //     this.locationContent = {}
    //     console.log('no more locations')
    //     this.quitGame()
    //   } else { this.showNextLocation() }
    // },
    showContentDialog () {
      this.contentDialog = true
    },

    quitGame () {
      this.quitPressed = true
      this.gameInProgress = false
      this.contentDialog = true
      this.GetWalk()
      console.log('quit')
    }
  }
}
</script>

<style>

</style>
