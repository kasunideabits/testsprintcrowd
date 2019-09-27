def image

pipeline {
  agent none
  environment
  {
    ECRURL = '989299900151.dkr.ecr.ap-southeast-1.amazonaws.com'
    ECRCRED = 'ecr:ap-southeast-1:aws-client-creds'
    REPOSITORY = 'sprintcrowd/backend'
  }
  stages {
    stage("build") {
        agent { label 'scrowd-slave' }
        when { anyOf { branch 'master'; branch 'development'; branch 'qa' } }
        steps {
            script {
                image = docker.build("${env.REPOSITORY}:${env.BRANCH_NAME}.${env.BUILD_ID}")
            }
        }
    }
    stage("push-image") {
        agent { label 'scrowd-slave' }
        when { anyOf { branch 'master'; branch 'development'; branch 'qa' } }
        steps {
            script {
                docker.withRegistry("https://${env.ECRURL}", ECRCRED) {
                    image.push("${env.BRANCH_NAME}.${env.BUILD_ID}")
                    image.push("${env.BRANCH_NAME}.latest")
                }
                sh "docker rmi -f ${env.ECRURL}/${env.REPOSITORY}:${env.BRANCH_NAME}.${env.BUILD_ID} ${env.ECRURL}/${env.REPOSITORY}:${env.BRANCH_NAME}.latest ${env.REPOSITORY}:${env.BRANCH_NAME}.${env.BUILD_ID}"
            }
        }
    }
    stage("deploy-dev") {
      agent { label 'scrowd-slave' }
      when {
            branch 'development'
      }
      steps {
        script {
            docker.withRegistry("https://${env.ECRURL}", ECRCRED) {
              sh 'cd ~/devops; git pull'
              sh 'cd ~/devops/sprintcrowd-backend/dev; chmod 744 ./deploy.sh; ./deploy.sh'
            }
        }
      }
    }
    stage("deploy-qa") {
      agent { label 'scrowd-qa' }
      when {
            branch 'qa'
      }
      steps {
        script {
            docker.withRegistry("https://${env.ECRURL}", ECRCRED) {
              sh 'cd ~/devops; git pull'
              sh 'cd ~/devops/sprintcrowd-backend/qa; chmod 744 ./deploy.sh; ./deploy.sh'
            }
        }
      }
    }
    stage("deploy-live") {
      agent { label 'scrowd-prod' }
      when {
            branch 'master'
      }
      steps {
        script {
            docker.withRegistry("https://${env.ECRURL}", ECRCRED) {
              sh 'cd ~/devops; git pull'
              sh 'cd ~/devops/sprintcrowd-backend/prod; chmod 744 ./deploy.sh; ./deploy.sh'
            }
        }
      }
    }
  }
}
