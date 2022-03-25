<template>
  <v-container>
    <h2>Walks</h2>
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
              Walk Id
            </th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(value, key) in walks" :key="key" @click="onRowClick(value.id)">
            <td>{{ key }} </td>
            <td>{{ value.walkName }} </td>
            <td>{{ 'length' }}</td>
            <td>{{ value.id }}</td>
            <td>
              <v-btn class="warning" @click.stop="EditWalk(value.id)">
                Edit
              </v-btn>
            </td>
            <td>
              <v-btn class="red" @click.stop="RemoveWalk(value.id)">
                Remove Walk
              </v-btn>
            </td>
          </tr>
        </tbody>
      </template>
    </v-simple-table>
  </v-container>
</template>

<script>

export default {
  data () {
    return {
      walks: null
    }
  },
  created () {
    this.getWalks()
  },
  methods: {
    getWalks () {
      this.$axios
        .get('/walk')
        .then((res) => {
          this.walks = res.data
        })
    },
    EditWalk (walkId) {
      this.$router.push({ path: `/EditWalk/${walkId}` })
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
      this.$router.push({ path: `/walk/${walkId}` })
    }
  }
}
</script>

<style>
.row-pointer > .v-data-table__wrapper > table > tbody > tr:hover {
  cursor: pointer;}

.ol-viewport {
  width:1000px;
  height: 1000px;
}

</style>
