import axios from 'axios'

const ENDPOINT = 'https://nominatim.openstreetmap.org/reverse'
const SEARCHENDPOINT = 'https://nominatim.openstreetmap.org/search'
const FORMAT = 'jsonv2'

export function currentCoordinates (manualLocation) {
  if (!manualLocation) {
    return new Promise((resolve, reject) => {
      navigator.geolocation.getCurrentPosition(
        ({ coords }) => resolve(coords),
        // Reject if the user doesn't
        // allow accessing their location.
        error => reject(error)
      )
    })
  } else {
    return new Promise((resolve, reject) => {
      navigator.geolocation.getCurrentPosition(
        ({ coords }) => resolve(coords),
        // Reject if the user doesn't
        // allow accessing their location.
        error => reject(error)
      )
    })
  }
}

export async function addressByCoordinates ({ latitude, longitude }) {
  const { data } = await axios.get(ENDPOINT, {
    params: {
      format: FORMAT,
      lat: latitude,
      lon: longitude
    }
  })
  data.address.coordinates = [latitude, longitude]
  return data.address
}
export async function currentAddress (manualLocation) {
  const coordinates = await currentCoordinates(manualLocation)
  return addressByCoordinates(coordinates)
}

async function coordinatesByAddress (manualLocation) {
  const { data } = await axios.get(SEARCHENDPOINT, {
    params: {
      format: FORMAT,
      street: manualLocation.street,
      city: manualLocation.city,
      country: manualLocation.country,
      postalcode: manualLocation.postalcode
    }
  })
  console.log(data)
  return data
}
export async function manualAddress (manualLocation) {
  const coordinates = await coordinatesByAddress(manualLocation)
  return coordinates
}
