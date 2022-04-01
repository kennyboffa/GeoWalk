<template>
  <div>
    <v-app-bar-nav-icon>
      <h4>
        <NuxtLink to="/walkspage">
          Admin Pages
        </NuxtLink>
      </h4>
      <h4>
        <NuxtLink to="/about">
          About
        </Nuxtlink>
      </h4>
    </v-app-bar-nav-icon>
    <h1>Welcome!</h1>
    <h2>
      <NuxtLink to="/game/setUserLocation">
        Play
      </Nuxtlink>
    </h2>
  </div>
</template>

<script>
export default {

}
</script>

<style>

</style>
