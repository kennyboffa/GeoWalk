<template>
  <div class="App o-container o-container--s o-vertical-spacing o-vertical-spacing--xl">
    <h1>Your Location</h1>

    <section class="App__example o-vertical-spacing o-vertical-spacing--l">
      <LocationFrame>
        <form
          slot-scope="{
            address,
            error,
            fetchAddress,
            geolocationSupported,
            loading,
          }"
          class="o-vertical-spacing"
        >
          <v-btn
            v-if="geolocationSupported = true"
            type="button"
            @click="[fetchAddress(), setRender()]"
          >
            {{ loading ? 'Loading ...' : 'Autofill my address' }}
          </v-btn>
          <br><br><br><div>OR enter address manually below</div><br>
          <div
            v-if="error"
            class="error"
          >
            There was an error fetching your location, please try again.
          </div>
          <div>
            <v-text-field
              v-model="address.road"
              name="street"
              label="Street"
            />

            <v-text-field
              v-model="address.house_number"
              name="street_number"
              label="Street Number"
            />
            <v-text-field
              v-model="address.postcode"
              name="postalcode"
              label="Postal Code"
            />

            <v-text-field
              v-model="address.town"
              name="town"
              label="Town"
              required
            />

            <v-text-field
              v-model="address.country"
              name="country"
              label="Country"
            />
          </div>
          <v-btn class="success" @click="[fetchAddress(address), setRender()]">
            Update Walks
          </v-btn>
        </form>
      </LocationFrame>
    </section>
  </div>
</template>

<script>
import LocationFrame from '../components/LocationFrame'

export default {
  name: 'App',
  components: {
    LocationFrame
  },
  data () {
    return {
      address: {
        road: undefined,
        house_number: undefined,
        country: undefined,
        town: undefined,
        postcode: undefined

      }
    }
  },
  methods: {

    SetManualAddress (address) {

    },
    setRender () {
      this.$emit('render')
    }
  }
}
</script>
