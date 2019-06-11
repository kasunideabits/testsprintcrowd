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
        agent { label 'LinuxSlave' }
        // when { anyOf { branch 'master'; branch 'development' } } //build every branch
        steps {
            script {
                image = docker.build("${env.REPOSITORY}:${env.BRANCH_NAME}.${env.BUILD_ID}")
            }
        }
    }
    // stage("test") {
    //     agent { label 'LinuxSlave' }
    //     steps {
    //         script {
    //             sh 'cd Tests/; dotnet restore; dotnet test'
    //         }
    //     }
    // }
    stage("push-image") {
        agent { label 'LinuxSlave' }
        when { anyOf { branch 'master'; branch 'development' } }
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
    stage("deploy") {
      agent { label 'scrowd-slave' }
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
