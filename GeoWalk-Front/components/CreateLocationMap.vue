<template>
  <v-container fluid>
    <v-dialog
      v-model="dialog"
      persistent
      max-width="600px"
    >
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
                  v-model="latitude"
                  label="Location Latitude"
                  required
                />
                <v-text-field
                  v-model="longitude"
                  label="Location Longitude"
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
  }
}
</script>

<style>

</style>
