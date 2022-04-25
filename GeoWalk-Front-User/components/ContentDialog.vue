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
            <v-card-text v-if="gameFinished || quitPressed">
              Your score: {{ totalScore }} Number of locations visited: {{ visitedLocations.length }}
            </v-card-text>
            <div v-if="!quitPressed && !gameFinished">
              <div
                v-for="(item,index) in baseContent"
                :key="index"
              >
                {{ item.title }}

                <v-card-text>{{ item.summarize }}</v-card-text>
                <div
                  v-for="(i, ind) in answers"
                  :key="ind"
                >
                  <v-radio-group
                    v-if="item.type === 'questionanswer'"
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
            </div>
          </v-col>
        </v-row>
      </v-container>
    </v-card-text>
    <v-card-actions>
      <v-spacer />
      <v-btn
        v-if="typeOfContent === 'questionanswer' && !gameFinished"
        color="green darken-1"
        text
        @click="[$emit('dialog-close'), addPoints(points)]"
      >
        Submit
      </v-btn>
      <v-btn
        v-if="typeOfContent === 'info' && !gameFinished"
        color="blue darken-1"
        text
        @click="$emit('dialog-close')"
      >
        Close
      </v-btn>
      <v-btn
        v-if="gameFinished"
        color="blue darken-1"
        text
        @click="$emit('dialog-close'), totalScore = 0"
      >
        Finish
      </v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
export default {
  props: {
    locationContent: { type: Object, default: null },
    quitPressed: { type: Boolean, default: false },
    visitedLocations: { type: Array, default: undefined }
  },
  data () {
    return {
      gameFinished: false,
      typeOfContent: undefined,
      radioGroup: 1,
      totalScore: null,
      points: null,
      baseContent: [],
      content: {},
      title: undefined,
      radioSelected: undefined,
      answers: [{

      }]
    }
  },
  watch: {

    locationContent (old, value) {
      this.gameFinished = false
      if (this.locationContent.id !== undefined) {
        this.GetLocationContent()
      } else {
        console.log('asd')
        this.gameFinished = true
      }
    }
  },
  mounted () {
    this.GetLocationContent()
  },
  methods: {
    async GetLocationContent () {
      await this.$axios
        .get(`/location/${this.locationContent.id}`)
        .then((res) => {
          this.GetContent(this.baseContent[0].id)
          this.GetAnswers(this.baseContent[0].id)
          this.typeOfContent = this.baseContent[0].type
        })
    },
    async GetAnswers (id) {
      await this.$axios
        .get(`/content/${id}/questionanswer`)
        .then((res) => {
          this.answers = res.data
        })
    },
    async GetContent (id) {
      await this.$axios
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
