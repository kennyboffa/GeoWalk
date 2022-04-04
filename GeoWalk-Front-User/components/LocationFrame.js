import * as locationService from '../services/location'

export default {
  data () {
    return {
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
    async fetchAddress () {
      try {
        // this.setLoadingState()
        this.address = await locationService.currentAddress()
        this.loading = false
      } catch (error) {
        this.setErrorState(error)
        console.log(error)
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
      gelocationSupported: this.geoLocationSupported,
      fetchAddress: this.fetchAddress
    })
  }
}
