<template>
  <v-card>
    <v-card-title>
      <span class="text-h5">Add Content</span>
    </v-card-title>
    <v-card-text>
      <v-container fluid>
        <v-row>
          <v-col
            cols="12"
            sm="12"
            md="12"
          >
            <div>Content Type for Location {{ contentLocationId }}</div>
            <v-radio-group
              v-model="radioButtonSelected"
              row
              mandatory
            >
              <v-radio
                v-model="radio1"
                label="Information/Fact"
                value="Info"
                @click="type='info'"
              />
              <v-radio
                v-model="radio2"
                label="Question/Answers"
                value="Question"
                @click="type='questionanswer'"
              />
            </v-radio-group>
            <v-text-field
              v-model="title"
              label="* Title"
              value=":this.title"
            />
            <div v-if="title === undefined || title === '' " class="dialog-required-message">
              Title has to be set
            </div>

            <v-text-field
              v-if="radioButtonSelected === radio1"
              v-model="info"
              label="Fact/Information"
              value=":this.info"
            />
            <v-text-field
              v-if="radioButtonSelected === radio2"
              v-model="question"
              label="Question"
              value=":this.question"
            />
            <div
              v-for="(answer,index) in answers"
              :key="index"
            >
              <v-text-field
                v-if="radioButtonSelected === radio2"
                v-model="answer.answerText"
                label="Answer"
              />
              <v-text-field
                v-if="radioButtonSelected === radio2"
                v-model="answer.points"
                class="points-field"
                label="Points"
                type="number"
              />
              <!-- <v-text-field
              v-if="radioButtonSelected === radio2"
              v-model="answers.two"
              label="Answer"
              value=":this.answers"
            />
            <v-text-field
              v-if="radioButtonSelected === radio2"
              v-model="answers.three"
              label="Answer"
              value=":this.answers"
            /> -->
              <!-- <div
              v-for="(answer,index) in answers"
              :key="index"
            >
              <v-text-field
                v-if="radioButtonSelected === radio2"
                v-model="answer.index"
                label="Answer"
                value=":this.answers"
              /> -->

              <v-btn class="red" @click="DeleteRow(index)">
                Delete Row
              </v-btn>
            </div>
          </v-col>
          <v-col
            cols="12"
            sm="6"
            md="4"
          />
        </v-row>
        <v-btn v-if="radioButtonSelected === radio2" @click="AddAnswerRow">
          Add Another Answer
        </v-btn>
      </v-container>
      <small>*indicates required field</small>
    </v-card-text>
    <v-card-actions>
      <v-spacer />
      <v-btn
        color="blue darken-1"
        text
        @click="$emit('dialog-close')"
      >
        Cancel
      </v-btn>
      <v-btn
        color="blue darken-1"
        text
        @click="AddContent()"
      >
        Save
      </v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
export default {
  props: {
    contentLocationId: { type: Number, default: null }
  },
  data () {
    return {
      contents: [],
      title: undefined,
      radio1: 'Info',
      radio2: 'Question',
      radioButtonSelected: 'Info',
      info: '',
      question: '',
      type: 'info',
      answers: [{

      }]
    }
  },

  methods: {

    AddContent () {
      const newContent = {
        locationId: this.contentLocationId,
        type: this.type,
        title: this.title,
        info: this.info,
        question: this.question,
        answers: this.answers

      }
      if (this.title !== undefined && this.title !== '') {
        this.$axios
          .post('/content/', newContent)
        this.dialog = false
        this.$emit('dialog-close')
        this.$emit('content-added')
        this.info = ''
        this.title = undefined
        this.question = ''
        this.answers = []
      }
    },
    AddAnswerRow () {
      this.answers.push({
      })
    },
    DeleteRow (index) {
      this.answers.splice(index, 1)
    }

    // GetContent () {
    //   this.$axios
    //     .get(`/location/${this.contentLocationId}`)
    //     .then((res) => {
    //       this.contents = res.data.contents
    //     })
    // }
  }
}

</script>

<style>
.dialog-required-message{
  font-size: 12px;
  color:red
}
</style>
