<template>
    <div>
        <h2 class="content-block">Tasks</h2>
        <div class="content-block">
            <div class="dx-card responsive-paddings">
                <DxDataGrid :data-source="dataSource"
                            :show-borders="true"
                            :columns="taskColumns"
                            key-expr="Id"
                            ref="myDataGrid">
                    <DxPaging :enabled="false" />

                    <DxEditing :allow-updating="true"
                               :allow-adding="true"
                               :allow-deleting="true"
                               mode="popup">

                        <DxPopup :show-title="true"
                                 :width="700"
                                 :height="525"
                                 title="Tasks Info" />
                        <DxForm>

                            <DxItem :col-count="2"
                                    :col-span="2"
                                    item-type="group">
                                <DxItem data-field="Subject" />
                                <DxItem data-field="Company" />
                                <DxItem data-field="Priority" />
                                <DxItem data-field="StartDate"
                                        caption="Start Date"
                                        data-type="date" />

                                <DxItem data-field="DueDate" caption="Due Date"
                                        data-type="date" />
                                <DxItem data-field="Owner" />

                                <DxItem data-field="Status" />

                            </DxItem>


                        </DxForm>
                    </DxEditing>


                    <DxColumn data-field="Subject" />
                    <DxColumn data-field="Company" />
                    <DxColumn data-field="Priority" />
                    <DxColumn data-field="StartDate"
                              data-type="date" />
                    <DxColumn data-field="DueDate"
                              data-type="date" />
                    <DxColumn data-field="Status" />

                </DxDataGrid>
            </div>
        </div>
    </div>
</template>
<script>

    import Vue from 'vue';
    import axios from 'axios';
    import CustomStore from 'devextreme/data/custom_store';
    import DxButton from 'devextreme-vue/button';
    import { useSignalR } from '@dreamonkey/vue-signalr';
    import { inject } from 'vue';

    import {
        DxDataGrid,
        DxColumn,
        DxHeaderFilter,
        DxLoadPanel,
        DxRequiredRule,
        DxSelection,
        DxScrolling,
        DxSorting,
        DxColumnLookup,
        DxPager,
        DxPaging,
        DxEditing,
        DxDataGridTypes,

    } from 'devextreme-vue/data-grid';

    function handleErrors(response) {
        if (!response.ok) {
            throw Error(response.statusText);
        }
        return response;
    }

    var serviceUrl = 'https://localhost:7282';

    export default {
        components: {
            DxDataGrid,
            DxButton,
            DxEditing
        },
        data() {
            return {
                dataSource: null,
                taskColumns: ['Subject', 'Company', 'Priority', 'StartDate', 'DueDate', 'Owner', 'Status'],

            };
        },
        mounted() {
            const signalr = useSignalR();

            signalr.on('RefreshGrid', (eventName) => {
                console.log(eventName);

                this.$refs['myDataGrid'].instance.refresh();

            });

            const store = new CustomStore({
                key: "Id",
                loadMode: 'raw',
                load: async function () {
                    var tasks = await axios.get('/api/Tasks/Get');
                    console.log(tasks.data);
                    return tasks.data;
                },
                insert: (values) => {
                    signalr.invoke('EventUpdate', "TaskList_Inserted");

                    const strValues = JSON.stringify(values);
                    return fetch(serviceUrl + `/api/Tasks/Post/?values=${encodeURIComponent(strValues)}`, {
                        method: 'POST',
                        body: JSON.stringify(values),
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    })
                        .then(handleErrors)
                        .catch(() => { throw 'Network error' });
                },

                remove: (key) => {
                    signalr.invoke('EventUpdate', "TaskList_Deleted");

                    return fetch(serviceUrl + `/api/Tasks/Delete/${encodeURIComponent(key)}`, {
                        method: 'DELETE'
                    })
                        .then(handleErrors)
                        .catch(() => { throw 'Network error' });
                },
                update: (key, values) => {
                    signalr.invoke('EventUpdate', "TaskList_Updated");

                    const strValues = JSON.stringify(values);

                    return fetch(serviceUrl + `/api/Tasks/Put/?key=${encodeURIComponent(key)}&values=${encodeURIComponent(strValues)}`, {
                        method: 'PUT',
                        body: JSON.stringify(values),
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    })
                        .then(handleErrors)
                        .catch(() => { throw 'Network error' });
                }
            });

            this.dataSource = store;
        },

    };
</script>
