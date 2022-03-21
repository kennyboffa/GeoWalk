<template>
  <v-container fluid>
    <v-dialog
      v-model="dialog"
      persistent
      max-width="600px"
    >
      <template #activator="{ on, attrs }">
        <v-btn
          color="primary"
          dark
          v-bind="attrs"
          v-on="on"
        >
          Add a Location
        </v-btn>
      </template>
      <v-card>
        <v-card-title>
          <span class="text-h5">Add a Location</span>
        </v-card-title>
        <v-card-text>
          <v-container>
            <v-row>
              <v-col
                cols="12"
                sm="6"
                md="4"
              >
                <v-text-field
                  v-model="longitude"
                  label="Location Longitude"
                  required
                />
                <v-text-field
                  v-model="latitude"
                  label="Location Latitude"
                  required
                />
              </v-col>
              <v-col
                cols="12"
                sm="6"
                md="4"
              />
            </v-row>
          </v-container>
          <small>*indicates required field</small>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn
            color="blue darken-1"
            text
            @click="dialog = false"
          >
            Close
          </v-btn>
          <v-btn
            color="blue darken-1"
            text
            @click="AddLocation(selectedWalkId, latitude, longitude), dialog = false"
          >
            Save
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script>
export default {
  props: [
    'selectedWalkId'
  ],

  data () {
    return {
      coordinates: '',
      latitude: '',
      longitude: '',
      dialog: false
    }
  },
  methods: {
    async AddLocation (selectedWalkId, latitude, longitude) {
      await this.$axios
        .post(`/location/${selectedWalkId} ${latitude} ${longitude}`)
        .then((res) => {
          this.coordinates = res.data
          this.$emit('location-added')
        })
    }
  }
}
</script>

<style>

</style>
