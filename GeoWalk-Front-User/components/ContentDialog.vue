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
              Content Type for Location {{ item.title }}

              <v-card-text>{{ item.summarize }}</v-card-text>
              <div
                v-for="(i, ind) in content.answers"
                :key="ind"
              >
                <v-radio
                  v-if="item.type == 'questionanswer'"
                  v-model="radio1"
                  label="Answer 1"
                  value="i"
                />
                <v-radio
                  v-if="item.type == 'questionanswer'"
                  v-model="radio2"
                  label="Answer 2"
                  value="Answer2"
                />
              </div>
            </div>
            <!-- <v-radio-group
              v-model="radioButtonSelected"
              row
              mandatory
            > -->

            <!-- </v-radio-group> -->

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
          </v-col>
          <v-col
            cols="12"
            sm="6"
            md="4"
          />
        </v-row>
      </v-container>
    </v-card-text>
    <v-card-actions>
      <v-spacer />
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
    locationContent: { type: Object, default: null }
  },
  data () {
    return {
      baseContent: [],
      content: [],
      title: undefined,
      radio1: undefined,
      radio2: undefined,
      info: '',
      question: '',
      type: 'info',
      answers: [{

      }]
    }
  },
  created () {
    this.GetLocationContent()
  },
  methods: {
    GetLocationContent () {
      this.$axios
        .get(`/location/${this.locationContent.id}`)
        .then((res) => {
          this.baseContent = res.data.contents
          console.log(this.baseContent)
          this.GetContent()
        })
    },
    GetContent () {
      this.$axios
        .get(`/content/${this.baseContent[0].id}`)
        .then((res) => {
          this.content = res.data
          console.log(this.content)
        })
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
