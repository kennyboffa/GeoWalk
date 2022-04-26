<template>
  <div class="App o-container o-container--s o-vertical-spacing o-vertical-spacing--xl">
    <h1>Your Location</h1>

    <section class="App__example o-vertical-spacing o-vertical-spacing--l">
      <LocationFrame>
        <form
          slot-scope="{
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
            @click="fetchAddress()"
          >
            {{ loading ? 'Loading ...' : 'Find My Location' }}
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
              v-model="address.street"
              name="street"
              label="Street"
            />

            <!-- <v-text-field
              v-model="address.house_number"
              name="street_number"
              label="Street Number"
            /> -->
            <v-text-field
              v-model="address.postalcode"
              name="postalcode"
              label="Postal Code"
            />

            <v-text-field
              v-model="address.city"
              name="city"
              label="City"
              required
            />

            <v-text-field
              v-model="address.country"
              name="country"
              label="Country"
            />
          </div>
          <v-btn class="success" @click="fetchAddress(address)">
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
        street: undefined,
        // house_number: undefined,
        country: undefined,
        city: undefined,
        postalcode: undefined

      }
    }
  },
  methods: {
  }
}
</script>
