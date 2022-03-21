<template>
  <v-app>
    <v-btn color="primary" @click="toggleDarkMode">
      Toggle Dark Mode
    </v-btn>
    <v-container fluid>
      <v-row justify="center" align="center">
        <v-col md="8" lg="8" sm="8" xs="8">
          <v-main class="content">
            <Nuxt />
          </v-main>
        </v-col>
      </v-row>
    </v-container>
  </v-app>
</template>

<script>
export default {

  methods: {
    toggleDarkMode () {
      this.$vuetify.theme.dark = !this.$vuetify.theme.dark
    }
  }
}
</script>
