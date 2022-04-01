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
              Nr of Questions/Info
            </th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(value, key) in walks" :key="key" @click="onRowClick(value.id)">
            <td>{{ key }} </td>
            <td>{{ value.walkName }} </td>
            <td>{{ 'length' }}</td>
            <td>{{ locations }}</td>
          </tr>
        </tbody>
      </template>
    </v-simple-table>
    <div class="map-container">
      <ClientOnly>
        <MapContainerUser
          ref="mapContainer"
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
  data () {
    return {
      walks: [],
      locations: undefined
    }
  },
  created () {
    this.GetWalks()
  },
  mounted () {
  },
  methods: {
    GetWalks () {
      this.$axios
        .get('/walk')
        .then((res) => {
          this.walks = res.data
        })
    },
    GetWalk (id) {
      console.log(id)
      this.$axios
        .get(`/walk/${id}`)
        .then((res) => {
          this.locations = res.data.locations
          console.log(this.locations)
          // this.$emit('walkId')
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
