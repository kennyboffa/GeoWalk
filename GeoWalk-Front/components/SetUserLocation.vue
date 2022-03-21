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
            @click="fetchAddress"
          >
            {{ loading ? 'Loading ...' : 'Autofill my address' }}
          </v-btn>

          <div
            v-if="error"
            class="error"
          >
            There was an error fetching your location, please try again.
          </div>

          <div>
            <v-text-field
              :value="address.road"
              name="street"
              label="Street"
            />

            <v-text-field
              :value="address.house_number"
              name="street_number"
              label="Street Number"
            />
            <v-text-field
              :value="address.postcode"
              name="postalcode"
              label="Postal Code"
            />

            <v-text-field
              :value="address.town"
              name="town"
              label="Town"
              required
            />

            <v-text-field
              :value="address.country"
              name="country"
              label="Country"
            />
          </div>
          <v-btn class="success">
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
  }
}
</script>
