<template>
  <v-card>
    <v-card-title>
      <span class="text-h5">Location Content</span>
    </v-card-title>
    <v-card-text>
      <v-container fluid>
        <v-row>
          <v-col
            cols="12"
            sm="12"
            md="12"
          >
            <div
              v-for="(item,index) in baseContent"
              :key="index"
            >
              {{ item.title }}

              <v-card-text>{{ item.summarize }} {{ totalScore }}</v-card-text>
              <div
                v-for="(i, ind) in answers"
                :key="ind"
              >
                <v-radio-group
                  v-if="item.type = 'questionanswer'"
                  v-model="points"
                >
                  <v-radio
                    :label="`${i.answerText}`"
                    :value="i.points"
                  />
                  <div>{{ i.points }}</div>
                </v-radio-group>
              </div>
            </div>
          </v-col>
        </v-row>
      </v-container>
    </v-card-text>
    <v-card-actions>
      <v-spacer />
      <v-btn
        color="green darken-1"
        text
        @click="[$emit('dialog-close'), addPoints(points)]"
      >
        Submit
      </v-btn>
      <v-btn
        color="blue darken-1"
        text
        @click="$emit('dialog-close')"
      >
        Close
      </v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
export default {
  props: {
    locationContent: { type: Object, default: null },
    activate: { type: Boolean, default: false }
  },
  data () {
    return {
      radioGroup: 1,
      totalScore: null,
      points: null,
      baseContent: [],
      content: [],
      title: undefined,
      radioSelected: undefined,
      info: '',
      question: '',
      type: 'info',
      answers: [{

      }]
    }
  },
  mounted () {
    this.$nuxt.$on('getContent', () => {
      this.GetLocationContent()
    })
  },
  methods: {
    GetLocationContent () {
      this.$axios
        .get(`/location/${this.locationContent.id}`)
        .then((res) => {
          console.log(this.baseContent)
          this.baseContent = res.data.contents
          this.GetContent(this.baseContent[0].id)
          this.GetAnswers(this.baseContent[0].id)
        })
    },
    GetAnswers (id) {
      this.$axios
        .get(`/content/${id}/questionanswer`)
        .then((res) => {
          this.answers = res.data
        })
    },
    GetContent (id) {
      this.$axios
        .get(`/content/${id}`)
        .then((res) => {
          this.content = res.data
        })
    },
    addPoints (points) {
      this.totalScore += points
    }
  }
}

</script>

<style>
.dialog-required-message{
  font-size: 12px;
  color:red
}
</style>
