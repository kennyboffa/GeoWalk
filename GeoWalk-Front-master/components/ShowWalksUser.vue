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
            <td>{{ '4' }}</td>
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
