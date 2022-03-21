<template>

  <v-container fluid>
     <v-dialog
      v-model="dialog"
      persistent
      max-width="600px"
    >
      <template v-slot:activator="{ on, attrs }">
        <v-btn
          color="primary"
          dark
          v-bind="attrs"
          v-on="on"
        >
       Add Content
        </v-btn>
      </template>
      <v-card>
        <v-card-title>
          <span class="text-h5">Add Content</span>
        </v-card-title>
        <v-card-text>
          <v-container>
            <v-row>
              <v-col
                cols="12"
                sm="6"
                md="4"
              >
                <v-text-field v-model="title"
                  label="Title"
                  required
                ></v-text-field>
              </v-col>
              <v-col
                cols="12"
                sm="6"
                md="4"
              >
              </v-col>
                    <v-col
                cols="12"
                sm="6"
                md="4"
              >
                <v-text-field v-model="info"
                  label="Info"
                  required
                ></v-text-field>
              </v-col>
              <v-col
                cols="12"
                sm="6"
                md="4"
              >
              </v-col>
            </v-row>
          </v-container>
          <small>*indicates required field</small>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="blue darken-1"
            text
            @click="dialog = false"
          >
            Close
          </v-btn>
          <v-btn @click="AddContent(title, info, selectedLocationId), dialog = false"
            color="blue darken-1"
            text
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
    'selectedLocationId'
  ],

  data () {
    return {
      title: '',
      info: '',
      dialog: false
    }
  },
  methods: {
    async AddContent (title, info, selectedLocationId) {
      await this.$axios
        .post(`/content/${title} ${info} ${selectedLocationId}`)
        .then((res) => {
          this.coordinates = res.data
          this.$emit('content-added')
        })
    }
  }
}
</script>

<style>

</style>
