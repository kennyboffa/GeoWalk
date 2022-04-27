export const state = () => ({
  map: undefined,
  user: undefined
})
export const mutations = {
  set_map (state, value) {
    state.map = value
  }
}

// export const state = () => ({
//   locations: undefined
// })
// export const mutations = {
//   set_locations (state, value) {
//     state.locations = value
//   }
// }
