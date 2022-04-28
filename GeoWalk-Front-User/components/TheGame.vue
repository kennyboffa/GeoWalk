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
      localTime: undefined,
      user: this.$store.user,
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
      setInterval(() => {
        if (this.gameInProgress) {
          this.$nuxt.$emit('gameRunning')
          console.log('running')
        }
      }, 2500)
    },
    // setTimeout(() => {
    //   while (this.gameInProgress) {
    //     this.$nuxt.$emit('gameRunning')
    //     console.log('running')
    //   }
    // }, 2500)

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
      this.updateUserPosition()
      this.showNextLocation()
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

        setTimeout(() => {
          this.closeToPoint = true
          if (this.closeToPoint) {
            this.locationDone = false// - if user is close enough to location open dialog

            this.showContentDialog()
          }
        }, 500)

        // logic to show content and if the location is completed, conditional etc to set the below
      } else if (this.locationDone && this.locations.length === 0) {
        this.showNext = false
        this.gameInProgress = false
        this.locationContent = {}
        console.log('no more locations')
        this.quitGame()
      }
    },
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
