<template>
  <div>
    <CreateWalk @walk-added="$refs.showWalks.getWalks()" />
    <ShowWalks ref="showWalks" @walk-added="$refs.showWalks.getWalks()" />
  </div>
</template>

<script>

export default {

}
</script>

<style>

</style>
