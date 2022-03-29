<template>
  <v-list>
    <h2>Edit Content</h2>
    <v-list-item>
      <v-list-item-content v-if="content">
        ID: {{ content.id }} <br><br>
        <!-- <v-btn class="warning">Edit</v-btn><br><br> -->
        <v-form @submit.prevent="onSubmit()">
          <v-container>
            <v-row>
              <v-col cols="12" md="4">
                <v-text-field
                  v-model="content.title"
                  label="Title"
                  required
                />
              </v-col>

              <v-col cols="12" md="4">
                <v-text-field
                  v-model="content.info"
                  label="Info"
                  required
                />
              </v-col>
              <v-col cols="12" md="4">
                <v-text-field
                  v-model="content.question"
                  label="Question"
                  required
                />
              </v-col>
              <v-col cols="12" md="4">
                <v-text-field
                  v-model="content.correctAnswer"
                  label="Correct Answer"
                  required
                />
              </v-col>
              <v-col cols="12" md="4">
                <v-text-field
                  v-model="content.answerTwo"
                  label="Answer"
                  required
                />
              </v-col>
              <v-col cols="12" md="4">
                <v-text-field
                  v-model="content.answerThree"
                  label="Answer"
                  required
                />
              </v-col>
            </v-row>
          </v-container>
        </v-form>
      </v-list-item-content>
    </v-list-item>
    <v-btn class="success" @click="onSubmit()">
      Update
    </v-btn>
    <v-btn class="blue" @click="goBack()">
      Back
    </v-btn>
  </v-list>
</template>

<script>
export default {
  props: [
    'selectedContentId'
  ],
  data () {
    return {
      content: {
        content: this.content,
        info: this.info,
        title: this.title
      }
    }
  },
  created () {
    this.GetContent()
  },
  methods: {
    GetContent () {
      this.$axios
        .get(`/content/${this.selectedContentId}`)
        .then((res) => {
          this.content = res.data
          console.log(this.content)
        })
    },
    onSubmit () {
      this.$axios
        .put(`/content/${this.selectedContentId}`, this.content)
        .then((res) => {
          this.$router.go(-1)
        })
    },
    goBack () {
      this.$router.go(-1)
    }
  }
}

</script>

<style>

</style>
