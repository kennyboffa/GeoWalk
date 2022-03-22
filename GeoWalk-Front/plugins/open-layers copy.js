import Map from 'ol/Map'
import View from 'ol/View'
import OSM from 'ol/source/OSM'
import VectorSource from 'ol/source/Vector'
import TileLayer from 'ol/layer/Tile'
import VectorLayer from 'ol/layer/Vector'
import GeoJSON from 'ol/format/GeoJSON'
import { fromLonLat, toLonLat } from 'ol/proj'
import { Circle } from 'ol/geom'
import Feature from 'ol/Feature'
import { Circle as CircleStyle, Fill, Stroke, Style } from 'ol/style'

export default (context, inject) => {
  const ol = {
    Map,
    View,
    source: {
      OSM,
      VectorSource
    },
    layer: {
      TileLayer,
      VectorLayer
    },
    format: {
      GeoJSON,
      fromLonLat,
      toLonLat
    }
  }

  let hoveredFeatures = []
  const allFeatures = []
  const labelTextStroke = 'rgba(255, 255, 255, 1)'
  let pointerOverFeature = null

  const circleStyled = new CircleStyle({
    radius: 8,
    fill: new Fill({
      color: 'rgba(150,150,150,0.7)'
    }),

    stroke: new Stroke({ color: 'blue', width: 1 })
  })

  const styles = {
    MultiPoint: new Style({
      image: circleStyled
    })
  }

  ol.createMap = (options) => {
    options = {
      view: new ol.View({
        center: options.centerView ?? ol.format.fromLonLat([16, 15]),
        zoom: 6
      }),
      ...options
    }

    const styleFunction = function (feature) {
      return styles[feature.getGeometry().getType()]
    }

    const map = new ol.Map(options)

    map.on('pointermove', (e) => {
      hoveredFeatures = []
      const featureOver = map.forEachFeatureAtPixel(e.pixel, (feature) => {
        feature.set('showLabel', 'true')
        hoveredFeatures.push(`${feature.values_.layerName}_label`)
        return feature
      })

      if (pointerOverFeature && pointerOverFeature !== featureOver) {
        pointerOverFeature.set('showLabel', true)
      }
      pointerOverFeature = featureOver
    })
    return {
      map,
      addPosition: (layerName, x, y) => {
        const feature = new ol.format.GeoJSON().readFeature(
          {
            type: 'Feature',
            properties: { layerName },
            geometry: {
              type: 'MultiPoint',
              coordinates:
              [
                [x, y]
              ]
            }
          }
        )
        const vectorLayer = new ol.layer.VectorLayer({
          name: layerName,
          style: styleFunction,
          source: new ol.source.VectorSource({
            features: [feature]

          })
        })
        map.addLayer(vectorLayer)
      },
      removeLayerFromMap (layerName) {
        const layer = map.getLayers().getArray().find(x => x.get('name') === layerName)
        map.removeLayer(layer)
        const layerLabel = map.getLayers().getArray().find(x => x.get('name') === `${layerName}_label`)
        map.removeLayer(layerLabel)
      },
      createLabel (layerName, lon, lat) {
        layerName = `${layerName}_label`
        const labelFeature = new Feature({
          geometry: new Circle(([lon, lat]), 0)
        })
        const renderLabelText = (ctx, lon, lat) => {
          ctx.fillStyle = 'rgba(50,150,85,1)' // text fill färg
          // ctx.strokeStyle = stroke
          ctx.lineWidth = 1
          ctx.textAlign = 'start'
          ctx.textBaseline = 'bottom'
          ctx.font = 'bold 15px roboto'
          // ctx.filter = 'drop-shadow(7px 7px 2px #e81)'
          if (hoveredFeatures.includes(layerName)) {
            ctx.fillText(layerName, lon, lat)
            // ctx.strokeText(layerName, lon, lat)
          }
        }

        labelFeature.setStyle(new Style({
          renderer (coordinates, state) {
            const [[x, y], [x1, y1]] = coordinates
            const ctx = state.context
            const dx = x1 - x
            const dy = y1 - y
            const radius = Math.sqrt(dx * dx + dy * dy)

            const innerRadius = 0
            const outerRadius = radius * 1.4 // fillgradienten

            const gradient = ctx.createRadialGradient(
              x,
              y,
              innerRadius,
              x,
              y,
              outerRadius
            )
            gradient.addColorStop(0, 'rgba(255,255,0,0)') // dessa två lägger in färgval i en gradient som kallas sen till fillstyle
            gradient.addColorStop(0, 'rgba(80,50,50,0.2)')
            ctx.beginPath() // verkar sätta inre gräns för fill
            ctx.arc(x, y, radius * 50, 0, 2 * Math.PI, true)
            // ctx.strokeStyle = 'rgba(80,50,50,0.5)' // sätter stroke färg
            // ctx.stroke() // ser till så stroke är aktiverad

            renderLabelText(ctx, x, y, labelFeature.get('label-color', labelTextStroke))
          }
        })
        )
        const labelLayer = new ol.layer.VectorLayer({
          name: layerName,
          source: new ol.source.VectorSource({
            features: [labelFeature]
          })
        })
        allFeatures.push(labelFeature)
        map.addLayer(labelLayer)
      }
    }
  }
  inject('ol', ol)
}
