<template>
  <div>
    <SetUserLocation @render="setRender()" />
    <ShowWalksUser :render="render" />
  </div>
</template>

<script>
export default {
  data () {
    return {
      render: undefined
    }
  },
  methods: {
    setRender () {
      this.render = true
    }
  }

}
</script>

<style>

</style>
