<template>
  <v-container>
    <h2>Content</h2>
    <!-- <v-list>
    <v-list-item v-for="(value, key) in location" :key="key">
      <v-list-item-content>
                  <td>ID: {{value.coordinates}} </td>
      </v-list-item-content>
            </v-list-item>
                </v-list> -->

    <v-simple-table class="row-pointer">
      <template #default>
        <thead>
          <tr>
            <th class="text-left">
              Content ID
            </th>
            <th class="text-left">
              Title
            </th>
            <th class="text-left">
              Info
            </th>
            <th class="text-left">
              Questions
            </th>
            <th class="text-left">
              Answers
            </th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(item, key) in contents" :key="key" @click="onRowClick(item.id)">
            <td> {{ item.id }} </td>
            <td> {{ item.title }} </td>
            <td> {{ item.info }} </td>
            <td> {{ "Question" }} </td>
            <td> {{ "Answer" }} </td>
            <!-- <td><v-btn class="warning" @click.stop="EditLocation(item.id)"> Edit </v-btn></td> -->
            <td>
              <v-btn class="red" @click.stop="RemoveContent(item.id)">
                Remove
              </v-btn>
            </td>
          </tr>
        </tbody>
      </template>
    </v-simple-table>
    <v-btn class="blue" @click="goBack()">
      Back
    </v-btn>
    <CreateContent :selected-location-id="$route.params.locationId" @content-added="GetLocation()" />
  </v-container>
</template>

<script>
export default {
  props: [
    'selectedLocationId'
  ],

  data () {
    return {
      location: this.location,
      contents: this.contents
    }
  },
  created () {
    this.GetLocation(this.locationId)
  },
  methods: {
    GetLocation () {
      this.$axios
        .get(`/location/${this.selectedLocationId}`)
        .then((res) => {
          this.location = res.data
          this.contents = res.data.contents
          this.$emit('locationId')
        })
    },

    RemoveContent (contentId) {
      this.$axios
        .delete(`/content/${contentId}`)
        .then((res) => {
          this.content = res.data
          this.GetLocation(this.theLocationId)
        })
    },
    onRowClick (contentId) {
      this.$router.push({ path: `/content/${contentId}` })
    },
    goBack () {
      this.$router.go(-1)
    }
  }
}
</script>

<style>
.row-pointer > .v-data-table__wrapper > table > tbody > tr:hover {
  cursor: pointer;}
</style>
