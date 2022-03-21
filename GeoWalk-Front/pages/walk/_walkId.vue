<template>
  <ShowLocations :selected-walk-id="walkId" @mapPointRemoved="$refs.ShowLocations.setMapState()" />
</template>

<script>
export default {
  computed: {
    walkId () {
      return parseInt(this.$route.params.walkId)
    }
  }
}
</script>

<style>

</style>
