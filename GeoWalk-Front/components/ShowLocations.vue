<template>
  <v-container>
    <v-dialog
      v-model="dialog"
      persistent
      max-width="600px"
    >
      <ContentDialog
        :content-location-id="contentLocationId"
        @dialog-close="dialog = false"
      />
    </v-dialog>

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
    <v-expansion-panels
      focusable
    >
      <v-expansion-panel
        v-for="(item,i) in locations"
        :key="i"
        accordion
        @click="onRowClick(item.id)"
      >
        <v-expansion-panel-header ripple>
          <div class="main-info">
            Title: {{ item.title }}
          </div>
          <div class="main-info">
            Location ID: {{ item.id }}
          </div>
          <div class="main-info">
            Longitude: {{ item.longitude }}
          </div>
          <div class="main-info">
            Latitude: {{ item.latitude }}
          </div>
          <div class="btn-container">
            <v-btn class="warning ep-btn" @click.stop="EditLocation(item.id)">
              Edit
            </v-btn>
            <v-btn class="red ep-btn" @click.stop="RemoveLocation(item.id)">
              Remove
            </v-btn>
          </div>
        </v-expansion-panel-header>
        <v-expansion-panel-content
          v-for="(content, key) in contents"
          :key="key"
        >
          <v-simple-table class="location-content">
            <tr>
              <td class="location-content-items">
                {{ content.title }}
              </td>
              <td class="location-content-items">
                {{ content.info }}
              </td>
              <td class="location-content-items">
                {{ content.question }}
              </td>
              <td class="location-content-items">
                {{ content.answers }}
              </td>
              <td>
                <v-btn
                  class="
              warning ep-btn"
                  @click.stop="EditContent(content.id)"
                >
                  Edit
                </v-btn>
              </td>
              <td>
                <v-btn class="red ep-btn" @click.stop="RemoveContent(content.id, item.id)">
                  <!-- Funkar inte än -->
                  Remove
                </v-btn>
              </td>
            </tr>
          </v-simple-table>
        </v-expansion-panel-content>
        <v-btn class="green" @click.stop="AddContentClick(item.id)">
          <!-- Funkar inte än -->
          Add Content
        </v-btn>
      </v-expansion-panel>
    </v-expansion-panels>

    <v-btn class="blue back-button" @click="goBack()">
      Back
    </v-btn>
    <!-- //problem om det inte finns några locations -->
    <div class="map-container">
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
    </div>
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
      contents: [],
      dialog: false,
      locationRemoved: null,
      walk: undefined,
      locations: undefined,
      contentLocationId: null

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
      // this.$router.push({ path: `/location/${locationId}` })
      // this.dialog = true
      this.contents = this.GetContent(locationId)
      // console.log(this.contents)
    },
    goBack () {
      this.$router.go(-1)
    },
    onFeatureClick (clickedFeature) {
      this.$router.push({ path: `/location/${clickedFeature}` })
    },
    GetContent (locationId) {
      this.$axios
        .get(`/location/${locationId}`)
        .then((res) => {
          this.contents = res.data.contents
        })
    },
    EditContent (id) {
      this.$router.push({ path: `/content/${id}` })
    },
    AddContentClick (locationId) {
      this.dialog = true
      this.contentLocationId = locationId
    },
    RemoveContent (contentId, locationId) {
      this.$axios
        .delete(`/content/${contentId}`)
        .then((res) => {
          this.content = res.data
          this.GetContent(locationId)
        })
    }
  }
}

</script>

<style>
.row-pointer > .v-data-table__wrapper > table > tbody > tr:hover {
  cursor: pointer;}
  .btn-container{
    display: flexbox;
    text-align:right;
    margin-left: auto;
  }
  .ep-btn {
display: flexbox;
    margin-left:20px;
    margin-right:40px;
  max-width: 6vw;
  }
  .main-info{
    padding:10px;
    display: inline-flex;
    width:170px;
    text-align:center;
    margin-left: auto;
    margin-right: auto;
  }
  .map-container{
    display: flexbox;
    padding:80px;
    width: auto;
  }
  .back-button{
    margin-top: 15px;
  }
  .location-content{

    margin:10px;
    max-width:100%;
    border-style: solid;
    border-color:rgba(98, 107, 151, 0.438);
  }
    .location-content-items{
    padding:20px;
    width: 50%;
    display: flexbox;

  }
</style>
