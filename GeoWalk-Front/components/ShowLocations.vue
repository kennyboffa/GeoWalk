<template>
  <v-container>
    <h2>Locations</h2>
    <v-list>
      <v-list-item v-if="walk">
        <v-list-item-content>
          <td>ID: {{ walk.id }} </td>
        </v-list-item-content>
        <v-list-item-content>
          <td>Walk Name: {{ walk.walkName }} </td>
        </v-list-item-content>
      </v-list-item>
    </v-list>

    <v-simple-table class="row-pointer">
      <template #default>
        <thead>
          <tr>
            <th class="text-left">
              Title
            </th>
            <th class="text-left">
              Location ID
            </th>
            <th class="text-left">
              Longitude
            </th>
            <th class="text-left">
              Latitude
            </th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(item, key) in locations" :key="key" @click="onRowClick(item.id)">
            <td> {{ item.title }} </td>
            <td> {{ item.id }} </td>
            <td> {{ item.latitude }} </td>
            <td> {{ item.longitude }} </td>
            <td>
              <v-btn class="warning" @click.stop="EditLocation(item.id)">
                Edit
              </v-btn>
            </td>
            <td>
              <v-btn class="red" @click.stop="RemoveLocation(item.id)">
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
    <!-- //problem om det inte finns några locations -->
    <ClientOnly v-if="locations">
      <MapContainer
        ref="mapContainer"
        :location-removed="locationRemoved"
        :selected-walk-id="selectedWalkId"
        :locations="locations"
        @location-added="addLocation, GetWalk()"
        @location-removed="GetWalk()"
      />
    </ClientOnly>
  </v-container>
</template>

<script>
import MapContainer from '../manualComponents/MapContainer.vue'
import 'ol/ol.css'

export default {
  components: {
    MapContainer
  },
  props: {
    selectedWalkId: { type: Number, default: null }
  },
  data () {
    return {
      locationRemoved: null,
      walk: undefined,
      locations: undefined
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
          this.walk = res.data
          this.locations = res.data.locations
          this.$emit('walkId')
        })
    },
    EditLocation (id) {
      this.$router.push({ path: `/EditLocation/${id}` })
    },

    RemoveLocation (locationId) {
      const indexToRemove = this.locations.findIndex(x => x.id === locationId)
      this.locations.splice(indexToRemove, 1)
      // this.$refs.mapContainer.RemoveLocation(locationId)
      this.$axios
        .delete(`/location/${locationId}`)
        .then((res) => {
          this.walkId = res.data
        })
      this.$emit('location-removed')
      this.locationRemoved = locationId
    },
    addLocation (location) {
      this.locations.push(location)
    },
    onRowClick (locationId) {
      this.$router.push({ path: `/location/${locationId}` })
    },
    goBack () {
      this.$router.go(-1)
    },
    onFeatureClick (clickedFeature) {
      this.$router.push({ path: `/location/${clickedFeature}` })
    }
  }
}
</script>

<style>
.row-pointer > .v-data-table__wrapper > table > tbody > tr:hover {
  cursor: pointer;}
</style>
