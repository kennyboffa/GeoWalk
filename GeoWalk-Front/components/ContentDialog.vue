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
              />
              <v-radio
                v-model="radio2"
                label="Question/Answers"
                value="Question"
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
            <v-text-field
              v-if="radioButtonSelected === radio2"
              v-model="answers"
              label="Answers"
              value=":this.answers"
            />
          </v-col>
          <v-col
            cols="12"
            sm="6"
            md="4"
          />
        </v-row>
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
        @click="AddContent(contentLocationId, title, info, question, answers)"
      >
        Save
      </v-btn>
      <div
        v-for="(content, key) in contents"
        :key="key"
      >
        <v-simple-table class="location-content">
          <tr>
            <td class="location-content-items">
              {{ content.title }}
            </td>
            <td class="location-content-items">
              {{ content.info }}
            </td>
            <td class="location-content-items">
              {{ content.question }}
            </td>
            <td class="location-content-items">
              {{ content.answers }}
            </td>
            <td>
              <v-btn
                class="
              warning ep-btn"
                @click.stop="EditContent(content.id)"
              >
                Edit
              </v-btn>
            </td>
            <td>
              <v-btn class="red ep-btn" @click.stop="RemoveContent(content.id, item.id)">
                Remove
              </v-btn>
            </td>
          </tr>
        </v-simple-table>
      </div>
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
      answers: '' // make into an array
    }
  },

  methods: {
    AddContent () {
      const newContent = {
        locationId: this.contentLocationId,
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
        this.info = ''
        this.title = undefined
        this.question = ''
        this.answers = ''
      }
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
