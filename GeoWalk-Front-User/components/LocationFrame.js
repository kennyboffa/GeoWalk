import * as locationService from '../services/location'

export default {
  data () {
    return {
      manualLocation: {},
      address: {},
      error: null,
      geolocationSupported: '',
      loading: false
    }
  },
  mounted () {
    this.geolocationSupported = 'geolocation' in window.navigator
  },
  methods: {
    async fetchAddress (manualLocation) {
      if (!manualLocation) {
        try {
          // this.setLoadingState()
          this.address = await locationService.currentAddress()
          this.loading = false
          this.$cookiz.set('userPosition', this.address)
          console.log('inte manual')
        } catch (error) {
          this.setErrorState(error)
          console.log(error)
        }
      } else {
        console.log(manualLocation)
        this.address = await locationService.manualAddress(manualLocation)
        this.loading = false
        this.$cookiz.set('userPosition', this.address)
        console.log('manual')

        // console.log(this.address)
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
