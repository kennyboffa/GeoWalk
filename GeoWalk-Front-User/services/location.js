import axios from 'axios'

const ENDPOINT = 'https://nominatim.openstreetmap.org/reverse'
const SEARCHENDPOINT = 'https://nominatim.openstreetmap.org/search'
const FORMAT = 'jsonv2'

export function currentCoordinates () {
  return new Promise((resolve, reject) => {
    navigator.geolocation.getCurrentPosition(
      ({ coords }) => resolve(coords),
      // Reject if the user doesn't
      // allow accessing their location.
      error => reject(error)
    )
  })
}

export async function addressByCoordinates ({ latitude, longitude }) {
  const { data } = await axios.get(ENDPOINT, {
    params: {
      format: FORMAT,
      lat: latitude,
      lon: longitude
    }
  })
  // data.address.coordinates = [latitude, longitude]
  return data
}
export async function currentAddress () {
  const coordinates = await currentCoordinates()
  return addressByCoordinates(coordinates)
}

async function coordinatesByAddress (manualLocation) {
  const { data } = await axios.get(SEARCHENDPOINT, {
    params: {
      street: manualLocation.street,
      city: manualLocation.city,
      country: manualLocation.country,
      postalcode: manualLocation.postalcode,
      format: FORMAT
    }
  })
  return data[0]
}
// async function coordinatesByAddress (manualLocation) {
//   console.log(manualLocation)
//   const { data } = await axios.get(`${SEARCHENDPOINT + manualLocation.street + ',' + manualLocation.postalcode}'${FORMAT}`)
//   console.log(data)
//   return data
// }
export async function manualAddress (manualLocation) {
  const coordinates = await coordinatesByAddress(manualLocation)
  return coordinates
}

// + manualLocation.street + '?' +
//     manualLocation.city + '?' +
//     manualLocation.country + '?' +
//     manualLocation.postalcode
