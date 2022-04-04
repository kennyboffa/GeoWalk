<template>
  <div>
    <v-btn class="success">
      Start the Game!
    </v-btn>
    <v-btn class="blue back-button" @click="goBack()">
      Back
    </v-btn>
    <div class="map-container">
      <ClientOnly>
        <MapContainerUser
          ref="mapContainer"
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
      zoom: 14
    }
  },
  created () {
    this.GetWalk()
  },
  methods: {
    GetWalk () {
      this.$axios
        .get(`/walk/${this.selectedWalkId}`)
        .then((res) => {
          this.walk = res.data
          this.locations = res.data.locations
          this.$emit('walkId')
        })
    },
    goBack () {
      this.$router.go(-1)
    }
  }
}
</script>

<style>

</style>
