<template>
  <v-list>
    <h2>Edit Location</h2>
    <v-list-item>
      <v-list-item-content v-if="location">
        ID: {{ location.id }} <br><br>
        Coordinates: {{ location.coordinates }}<br><br>

        <v-form @submit.prevent="onSubmit()">
          <v-container>
            <v-row>
              <v-col cols="12" md="4">
                <v-text-field
                  v-model="location.title"
                  label="Title"
                  required
                />
                <v-text-field
                  v-model="location.longitude"
                  label="Longitude"
                  required
                />
                <v-text-field
                  v-model="location.latitude"
                  label="Latitude"
                  required
                />
              </v-col>
            </v-row>
          </v-container>
        </v-form>
      </v-list-item-content>
    </v-list-item>
    <v-btn class="success" @click="onSubmit()">
      Update
    </v-btn>
    <v-btn class="blue" @click="goBack()">
      Back
    </v-btn>
  </v-list>
</template>

<script>
export default {
  props: [
    'selectedLocationId'
  ],
  data () {
    return {
      location: {
        location: this.location,
        longitude: this.longitude,
        latitude: this.latitude,
        title: this.title
      }
    }
  },
  created () {
    this.GetLocation()
  },
  methods: {
    GetLocation () {
      this.$axios
        .get(`/location/${this.selectedLocationId}`)
        .then((res) => {
          this.location = res.data
        })
    },
    onSubmit () {
      this.$axios
        .put(`/location/${this.selectedLocationId}`, this.location)
        .then((res) => {
          this.$router.go(-1)
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
