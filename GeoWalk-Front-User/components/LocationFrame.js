import * as locationService from '../services/location'

export default {
  data () {
    return {
      address: {},
      error: null,
      geolocationSupported: undefined,
      loading: false,
      noReCenter: false

    }
  },
  mounted () {
    this.geolocationSupported = 'geolocation' in window.navigator
    this.$nuxt.$on('gameRunning', () => {
      this.fetchAddress()
      this.noReCenter = true
    })
  },
  methods: {

    async fetchAddress (manualLocation) {
      if (!manualLocation) {
        try {
          // this.setLoadingState()
          this.address = await locationService.currentAddress()
          this.loading = false
        } catch (error) {
          this.setErrorState(error)
          console.log(error)
        }
      } else {
        this.address = await locationService.manualAddress(manualLocation)
        this.loading = false
      }

      this.$cookiz.set('userPosition', this.address, { maxAge: 60 * 60 })
      if (!this.noReCenter) {
        this.$nuxt.$emit('updateCenterView')
      }
    },
    setErrorState () { // lägg till error
      this.error = null
      this.loading = true
    }
  },
  render () {
    return this.$scopedSlots.default({
      address: this.address,
      error: this.error,
      geolocationSupported: this.geoLocationSupported,
      fetchAddress: this.fetchAddress

    })
  }
}
