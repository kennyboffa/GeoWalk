<template>
<v-list>
  <h2>Edit Walk</h2>
  <v-list-item>
    <v-list-item-content v-if="walk">
            ID: {{walk.id}} <br><br>
      Name: {{walk.walkName}}<br><br>

      <v-form v-on:submit.prevent="onSubmit()">
        <v-container>
          <v-row>
            <v-col cols="12" md="4">
              <v-text-field
              v-model="walk.walkName"
              label="Walk Name"
              required>
              </v-text-field>
            </v-col>

            </v-row>
        </v-container>
      </v-form>
    </v-list-item-content>
  </v-list-item>
        <v-btn class="success" @click="onSubmit()">Update</v-btn>
            <v-btn class="blue" @click="goBack()">Back</v-btn>
</v-list>
</template>

<script>
export default {
  props: [
    'selectedWalkId'
  ],
  data () {
    return {
      walk: {
        walk: this.walk,
        walkName: this.walkName
      }
    }
  },
  created () {
    this.GetWalk()
  },
  methods: {
    GetWalk () {
      this.$axios
        .get(`/walk/${this.selectedWalkId}`)
        .then((res) => {
          this.walk = res.data[0]
        })
    },
    onSubmit () {
      this.$axios
        .put(`/walk/${this.selectedWalkId}`, this.walk)
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
