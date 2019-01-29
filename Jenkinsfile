def image

pipeline {
    agent { label 'LinuxSlave' }
    environment
    {
      ECRURL = '989299900151.dkr.ecr.ap-south-1.amazonaws.com'
      ECRCRED = 'ecr:ap-south-1:aws-client-creds'
      REPOSITORY = 'zacceleration/sprintcrowd'
    }
    stages {
        stage("build") {
            when { anyOf { branch 'master'; branch 'development' } }
            steps {
                script {
                    image = docker.build("${env.REPOSITORY}:${env.BRANCH_NAME}.${env.BUILD_ID}")
                }
            }
        }
        stage("push-image") {
            when { anyOf { branch 'master'; branch 'development' } }
            steps {
                script {
                    docker.withRegistry("https://${env.ECRURL}", ECRCRED) {
                        image.push("${env.BRANCH_NAME}.${env.BUILD_ID}")
                        image.push("${env.BRANCH_NAME}.latest")
                    }
                   sh "docker rmi -f ${env.ECRURL}/${env.REPOSITORY}:${env.BRANCH_NAME}.${env.BUILD_ID} ${env.ECRURL}/${env.REPOSITORY}:${env.BRANCH_NAME}.latest"
                }
            }
        }
    }
}
