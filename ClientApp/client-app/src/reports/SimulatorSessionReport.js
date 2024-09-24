import React from 'react';
import { Page, Text, View, Document, StyleSheet, Font } from '@react-pdf/renderer';
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
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
    },

    content__section: {
        width: '100%',
        display: "flex",
        alignItems: "start",
        justifyContent: "start",
        marginTop: '30px',
        marginBottom: '5px',
    },

    content__row: {
        width: "100%",
        border: "1px solid black",
        padding: "5px",
        display: "flex",
    },

    content__name: {
        fontWeight: 'bold',
    },

    footer: {
        width: "100%",
        display: "flex",
        flexDirection: "column",
        alignItems: "flex-end",
        justifyContent: "flex-end",
        marginTop: "40px",
        marginRight: '30px',
    },

    footer__sign: {
        width: "150px",
        height: "100px",
        borderBottom: "1px dotted black",
    },

    footer__name: {
        marginTop: '10px',
    }
});

const SimulatorSessionReport = ({ session }) => (
    <Document>
        <Page size="A4" style={styles.page}>

            <View style={styles.header}>
                <Text style={styles.header__title}>Raport Sesji Symulatorowej</Text>
                <Text style={styles.header__id}>#{session.id}</Text>
            </View>

            <View style={styles.content}>
                <Text style={styles.content__section}>Dane sesji:</Text>

                <View style={styles.content__row}>
                    <Text style={styles.content__name}>Rodzaj sesji:</Text>
                    <Text style={styles.content__value}>{session.category}</Text>
                </View>

                <View style={styles.content__row}>
                    <Text style={styles.content__name}>Nazwa:</Text>
                    <Text style={styles.content__value}>{session.name} ({session.abbreviation})</Text>
                </View>

                <View style={styles.content__row}>
                    <Text style={styles.content__name}>Opis:</Text>
                    <Text style={styles.content__value}>{session.description}</Text>
                </View>

                <Text style={styles.content__section}>Ramy czasowe:</Text>

                <View style={styles.content__row}>
                    <Text style={styles.content__name}>Godzina rozpoczęcia:</Text>
                    <Text style={styles.content__value}>{session.beginDate}</Text>
                </View>

                <View style={styles.content__row}>
                    <Text style={styles.content__name}>Godzina zakończenia:</Text>
                    <Text style={styles.content__value}>{session.endDate}</Text>
                </View>

                <View style={styles.content__row}>
                    <Text style={styles.content__name}>Czas trwania:</Text>
                    <Text style={styles.content__value}>{session.duration}</Text>
                </View>

                <Text style={styles.content__section}>Załoga:</Text>

                <View style={styles.content__row}>
                    <Text style={styles.content__name}>Pilot:</Text>
                    <Text style={styles.content__value}>{session.pilotName}</Text>
                </View>

                <View style={styles.content__row}>
                    <Text style={styles.content__name}>Copilot:</Text>
                    <Text style={styles.content__value}>{session.copilotName}</Text>
                </View>

                <View style={styles.content__row}>
                    <Text style={styles.content__name}>Obserwator:</Text>
                    <Text style={styles.content__value}>{session.observerName}</Text>
                </View>

                <View style={styles.content__row}>
                    <Text style={styles.content__name}>Instruktor:</Text>
                    <Text style={styles.content__value}>{session.supervisorName}</Text>
                </View>
            </View>

            <View style={styles.footer}>
                <View style={styles.footer__sign}></View>
                <Text style={styles.footer__name}>Podpis</Text>
            </View>

        </Page>
    </Document>
);

export default SimulatorSessionReport;
