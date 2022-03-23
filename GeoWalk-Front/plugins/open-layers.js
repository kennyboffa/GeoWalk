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
  // const zoomValue = null
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

  const styleFunction = function (feature) {
    return styles[feature.getGeometry().getType()] // Checks what type of feature it is and sets the style accordingly
  }

  ol.createMap = (options) => {
    options = {
      view: new ol.View({
        center: options.centerView ?? ol.format.fromLonLat([16, 15]),
        zoom: 6
      }),
      ...options
    }
    const map = new ol.Map(options) // Creates the map with above options

    return {
      map,
      addLayer: (layerId, typeOfLayer, x, y) => {
        const feature = new ol.format.GeoJSON().readFeature(
          {
            type: 'Feature',
            properties: { layerId, typeOfLayer },
            style: styleFunction,
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
          id: layerId,
          type: typeOfLayer,

          style: styleFunction,
          source: new ol.source.VectorSource({
            features: [feature]

          })
        })
        map.addLayer(vectorLayer)
        return (vectorLayer)
      },
      addPosition (layer, title, content) {
        layer.title = title
        layer.content = content

        // layor.addFeature(feature)

        // layer.on("pointermove")
        // feature.on("hover", () => {})
      },
      removeLayerFromMap (layerId) {
        const layer = map.getLayers().getArray().find(x => x.get('id') === layerId)
        map.removeLayer(layer)
        console.log(layer.title)
        const layerLabel = map.getLayers().getArray().find(x => x.get('id') === `${layer.title}_label`)
        map.removeLayer(layerLabel)
      },
      createLabel (layer, lon, lat) {
        console.log(layer.title)
        layer.title = `${layer.title}_label`
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
          if (hoveredFeatures.includes(layer.values_.id)) {
            ctx.fillText(layer.title, lon, lat)
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
        map.on('pointermove', (e) => {
          hoveredFeatures = []
          const featureOver = map.forEachFeatureAtPixel(e.pixel, (feature) => {
            feature.set('showLabel', 'true')
            hoveredFeatures.push(feature.values_.layerId)
            return feature
          })

          if (pointerOverFeature && pointerOverFeature !== featureOver) {
            pointerOverFeature.set('showLabel', true)
          }
          pointerOverFeature = featureOver
        })
        // this.zoomValue = map.getView().values_.zoom
        // map.getView().on('change:resolution', (e) => {
        //   const newZoomValue = e.target.values_.zoom
        //   if (newZoomValue > zoomValue) {
        //     console.log('smaller')
        //     this.zoomValue = newZoomValue
        //   } else {
        //     console.log('larger')
        //     this.zoomValue = newZoomValue
        //   }
        // })
        const labelLayer = new ol.layer.VectorLayer({
          name: layer.title,
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
