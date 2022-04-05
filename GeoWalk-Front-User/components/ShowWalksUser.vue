<template>
  <v-container>
    <h2>Walks Near You</h2>
    <v-simple-table class="row-pointer">
      <template #default>
        <thead>
          <tr>
            <th class="text-left">
              Nr
            </th>
            <th class="text-left">
              Name
            </th>
            <th class="text-left">
              Length in Km
            </th>
            <th class="text-left">
              Start Location
            </th>
            <th class="text-left">
              Nr of Locations
            </th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(value, key) in walks" :key="key" @click="onRowClick(value.id)">
            <td>{{ value.id }} </td>
            <td>{{ value.walkName }} </td>
            <td>{{ 'length' }}</td>
            <td>{{ value.locations[0].title }}</td>
            <td>{{ value.locations.length }}</td>
          </tr>
        </tbody>
      </template>
    </v-simple-table>
    <div class="map-container">
      <ClientOnly>
        <MapContainerUser
          ref="mapContainer"
          :render="render"
          :locations="locations"
          :walks="walks"
          :set-walk="setWalk"
        />
      </ClientOnly>
    </div>
  </v-container>
</template>

<script>

import MapContainerUser from '../manualComponents/MapContainerUser.vue'
import 'ol/ol.css'
export default {
  components: {
    MapContainerUser
  },

  props: {
    render: { type: Boolean, default: false }
  },
  data () {
    return {
      walks: [],
      locations: [],
      setWalk: []
    }
  },
  created () {
    this.GetWalks()
  },
  methods: {

    GetWalks () {
      this.$axios
        .get('/walk')
        .then((res) => {
          this.walks = res.data
          this.walks.forEach((element) => {
            this.locations.push(element.locations[0])
            // this.setWalk.push(element.id)
            // console.log(element.id)
          })
        })
    },
    GetWalk (id) {
      this.$axios
        .get(`/walk/${id}`)
        .then((res) => {
        })
    },
    EditWalk (id) {
      this.$router.push({ path: `/EditWalk/${id}` })
    },
    RemoveWalk (walkId) {
      this.$axios
        .delete(`/walk/${walkId}`)
        .then((res) => {
          this.walkId = res.data
          this.$emit('walk-added')
        })
    },
    onRowClick (walkId) {
      this.$router.push({ path: `./${walkId}` })
    }
  }
}
</script>

<style>
.row-pointer > .v-data-table__wrapper > table > tbody > tr:hover {
  cursor: pointer;}
</style>
