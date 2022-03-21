<template>
  <div> The pre-Game component!
  -  -  Show Map with the locations -- start button leading to game component-- back button
      </div>
</template>

<script>
export default {
  props: [
    'selectedWalkId'
  ],
  data () {
    return {
      locations: this.locations
    }
  },
  created () {
    console.log(this.selectedWalkId)
    this.GetWalk()
  },
  methods: {
    GetWalk () {
      this.$axios
        .get(`/walk/${this.selectedWalkId}`)
        .then((res) => {
          this.walk = res.data
          this.locations = res.data[0].locations
          this.$emit('walkId')
        })
    }
  }
}
</script>

<style>

</style>
