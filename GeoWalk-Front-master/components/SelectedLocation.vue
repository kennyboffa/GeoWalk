<template>
  <v-container>
    <v-list>
      <v-list-item v-for="(value, key) in location" :key="key">
        <v-list-item-content>
          <td>ID: {{ value.id }} </td>
        </v-list-item-content>
      </v-list-item>
    </v-list>

    <v-simple-table class="row-pointer">
      <template #default>
        <thead>
          <tr>
            <th class="text-left">
              Content ID
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
          <tr v-for="(item, key) in content" :key="key" @click="onRowClick(item.id)">
            <td> {{ item.id }} </td>
            <td> {{ item.content }} </td>
            <td> {{ item.content }} </td>
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
    <v-btn class="blue">
      Back
    </v-btn>
    <CreateContent :selected-content-id="$route.params.locationId" @content-added="GetContent(contentId)" />
  </v-container>
</template>

<script>
export default {
  props: [
    'selectedContentId'
  ],

  data () {
    return {
      content: this.content

    }
  },

  created () {
    console.log('aasdasd')
    this.GetContent(this.contentId)
  },
  methods: {
    GetContent () {
      this.$axios
        .get(`https://localhost:44393/GetContent${this.selectedContentId}`)
        .then((res) => {
          console.log(res)
          this.content = res.data
          this.$emit('contentId')
        })
    },

    RemoveContent (contentId) {
      this.$axios
        .delete(`https://localhost:44393/DeleteContent${contentId}`) // 7284
        .then((res) => {
          this.content = res.data
          this.getContent(this.contentId)
        })
    },
    onRowClick (contentId) {
      console.log('asd')
      this.$router.push({ path: `/content/${contentId}` })
    }
  }
}
</script>

<style>
.row-pointer > .v-data-table__wrapper > table > tbody > tr:hover {
  cursor: pointer;}
</style>
