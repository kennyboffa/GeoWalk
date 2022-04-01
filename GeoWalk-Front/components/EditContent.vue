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
                  v-model="baseContent.title"
                  label="Title"
                  required
                />
              </v-col>

              <v-col cols="12" md="4">
                <v-text-field
                  v-if="content.info"
                  v-model="content.info"
                  label="Info"
                  required
                />
              </v-col>
              <v-col cols="12" md="4">
                <v-text-field
                  v-if="!content.info"
                  v-model="baseContent.question"
                  label="Question"
                  required
                />
              </v-col>
              <v-col cols="12" md="4">
                <div
                  v-for="(item,index) in content"
                  :key="index"
                >
                  <v-text-field
                    v-if="!content.info"
                    v-model="item.answerText"
                    label="Answer"
                  />
                  <v-text-field
                    v-if="!content.info"
                    v-model="item.points"
                    class="points-field"
                    label="Points"
                    type="number"
                  />
                </div>
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
      baseContent: [{

      }],
      content: [{
        info: this.info,
        answers: this.answers
      }]
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
          this.baseContent = res.data

          this.GetContentByType(this.selectedContentId, res.data.type)
        })
    },

    GetContentByType (id, type) {
      this.$axios.get(`/content/${id}/${type}`)
        .then((res) => {
          this.content = res.data
        })
    },
    onSubmit () {
      const newContent = {
        title: this.baseContent.title,
        info: this.content.info,
        question: this.baseContent.question,
        answers: this.content

      }

      this.$axios
        .put(`/content/${this.selectedContentId}`, newContent)
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
