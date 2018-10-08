const repos = [
    {
        id: "1",
        name: "first",
        createdDate: "2018-10-03 16:03"
    },
    {
        id: "2",
        name: "second",
        createdDate: "2018-10-04 09:20"
    },
    {
        id: "3",
        name: "aaa",
        createdDate: "2017-10-04 09:20"
    }
];

export default class ContainerRepositoryService {
    getAll() {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                const copyOfRepos = JSON.parse(JSON.stringify(repos));
                resolve(copyOfRepos);
            }, 2000);
        });
    }

    add(repositoryData) {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                const newRepository = {
                    id: new Date().getTime(),
                    name: repositoryData.name,
                    createdDate: "2017-10-04 09:20"
                };

                repos.push(newRepository);
                resolve(newRepository);
            }, 2000);
        });
    }
}

export const service = new ContainerRepositoryService();