import React from 'react';
import { Page, Text, View, Document, StyleSheet, Font, Image } from '@react-pdf/renderer';
import robotoRegular from './../fonts/Roboto-Regular.ttf';
import robotoBold from './../fonts/Roboto-Bold.ttf';

Font.register({
    family: 'Roboto',
    fonts: [
        { src: robotoRegular, fontWeight: 'normal' },
        { src: robotoBold, fontWeight: 'bold' },
    ],
});

const styles = StyleSheet.create({
    page: {
        padding: '30px',
        fontSize: '12px',
        fontFamily: 'Roboto',
    },

    header: {
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'center',
        alignItems: 'center',
    },

    header__image: {
        width: '45px',
        height: 'auto',
        marginBottom: '5px',
    },

    header__title: {
        fontSize: '18px',
        marginBottom: '10px',
        fontWeight: 'bold',
        textAlign: 'center',
    },

    header__id: {
        fontSize: '16px',
        marginBottom: '10px',
        fontWeight: 'bold',
        textAlign: 'center',
    },

    content: {
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
    },

    content__section: {
        width: '100%',
        display: 'flex',
        alignItems: 'start',
        justifyContent: 'start',
        marginTop: '20px',
        marginBottom: '5px',
    },

    content__row: {
        width: '100%',
        border: '1px solid black',
        padding: '5px',
        display: 'flex',
        backgroundColor: 'lightgray',
    },

    content__name: {
        fontWeight: 'bold',
    },

    footer: {
        width: '100%',
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'flex-end',
        justifyContent: 'flex-end',
        marginTop: '20px',
        marginRight: '30px',
    },

    footer__sign: {
        width: '150px',
        height: '100px',
        borderBottom: '1px dotted black',
    },

    footer__name: {
        marginTop: '10px',
    }
});

const WorktimeSummaryReport = ({ user, sessions, statistics }) => (
    <Document>
        <Page size="A4" style={styles.page}>

            <View style={styles.header}>
                <Image style={styles.header__image} source={{ uri: './../../session.png' }}></Image>
                <Text style={styles.header__title}>Zestawienie Sesji Symulatorowych</Text>
                <Text style={styles.header__id}>{user.firstName} {user.lastName}</Text>
            </View>

            <View style={styles.content}>
                <Text style={styles.content__section}>Dane dotyczące czasu pracy</Text>

                <View style={styles.content__row}>
                    <Text style={styles.content__name}>Liczba zrealizowanych sesji jako uczeń</Text>
                    <Text style={styles.content__value}>{statistics.sessionsAsTrained}</Text>
                </View>

                <View style={styles.content__row}>
                    <Text style={styles.content__name}>Czas sesji jako uczeń</Text>
                    <Text style={styles.content__value}>{statistics.sessionsAsTrainedDuration} minut</Text>
                </View>

                <View style={styles.content__row}>
                    <Text style={styles.content__name}>Liczba zrealizowanych sesji jako instruktor</Text>
                    <Text style={styles.content__value}>{statistics.sessionsAsSupervisor}</Text>
                </View>

                <View style={styles.content__row}>
                    <Text style={styles.content__name}>Czas sesji jako instruktor</Text>
                    <Text style={styles.content__value}>{statistics.sessionsAsSupervisorDuration} minut</Text>
                </View>

                <Text style={styles.content__section}>Zestawienie sesji</Text>
                {sessions.map((session, index) => (
                    <View key={index} style={styles.content__row}>
                        <Text style={styles.content__name}>Sesja: {session.id} [{session.abbreviation}]</Text>
                        <Text style={styles.content__value}>{session.name}</Text>
                        <Text style={styles.content__value}>{session.beginDate} - {session.endDate}</Text>
                    </View>
                ))}

            </View>

            <View style={styles.footer}>
                <View style={styles.footer__sign}></View>
                <Text style={styles.footer__name}>Podpis</Text>
            </View>

        </Page>
    </Document>
);

export default WorktimeSummaryReport;
