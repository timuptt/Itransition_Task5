// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
{
    const baseUrl = "https://fakeuserdata.up.railway.app";
    const dataTable =  document.getElementById("dataTable");
    const mistakesSlider = document.getElementById("errorsCountSlider");
    const mistakesInput = document.getElementById("errorsCountText");
    const regionSelect = document.getElementById("regionInput");
    const table = document.getElementById("dataTable").getElementsByTagName('tbody')[0];
    const seedInput = document.getElementById("seedInput");
    const randomSeed = document.getElementById("randomSeed");

    let currentPage = 1;
    let currentData = [];

    const getRandomInt = (max) =>{
        return Math.floor(Math.random() * max);
    }

    const getData = async () =>{
        let url = new URL("Home/GetData/", baseUrl);
        url.searchParams.append("region", regionSelect.value);
        url.searchParams.append("mistakesRate", mistakesInput.value);
        url.searchParams.append("pageNumber", currentPage);
        url.searchParams.append("seed", seedInput.value);

        let response = await fetch(url)
            .then(response => response.json());

        response.forEach((item) => {
            currentData.push(item);
            let row = table.insertRow(dataTable.rows.length - 1);

            let numberCell = row.insertCell();
            numberCell.append(document.createTextNode(currentData.length))

            let idCell = row.insertCell();
            idCell.appendChild(document.createTextNode(item.id));

            let nameCell = row.insertCell();
            nameCell.appendChild(document.createTextNode(item.fullName));

            let addressCell = row.insertCell();
            addressCell.appendChild(document.createTextNode(`${item.addressString}`));

            let phoneCell = row.insertCell();
            phoneCell.appendChild(document.createTextNode(item.phoneNumber));
        });

        currentPage++;
    }

    const getCsv = async () =>{
        let url = new URL("Home/CreateCsv", baseUrl);

        if (currentData.length > 0){
            await fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json;charset=utf-8'
                },
                body: JSON.stringify(currentData)
            }).then(response => response.blob())
                .then(blob => {
                    let file = window.URL.createObjectURL(blob);
                    window.location.assign(file);
                });
        }
    }

    handleInputChange = async () => {
        document.getElementsByTagName("tbody")[0].innerHTML = "";
        currentPage = 1;
        currentData = [];

        await getData();
    }

    window.addEventListener('scroll', async () => {
        if (window.innerHeight + document.documentElement.scrollTop !== document.documentElement.offsetHeight) {
            return;
        } else {
            await getData();
        }
    });

    window.addEventListener('load', async () => {
        await getData();
    });

    mistakesInput.oninput = async (event) => {
        if (Number.isInteger(Number(event.target.value))) {
            mistakesSlider.value = event.target.value / 10;
        } else {
            mistakesSlider.value = 0;
        }

        await handleInputChange();
    }

    regionSelect.onchange = async () => {
        await handleInputChange();
    }

    mistakesSlider.onclick = async (event) => {
        mistakesInput.value = event.target.value;
        await handleInputChange();
    }

    mistakesInput.oninput = async () => {
        if (mistakesInput.value === "") {
            mistakesSlider.value = 0;
        }
        if (mistakesInput.value <= 10) {
            mistakesSlider.value = mistakesInput.value
        }
        await handleInputChange();
    }

    seedInput.oninput = async () => {
        await handleInputChange();
    }

    randomSeed.onclick = async () => {
        seedInput.value = getRandomInt(9999999);
        await handleInputChange();
    }

    createCsv.onclick = async () =>{
        await getCsv();
    }
}
